using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies = 10;
    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);
    public float checkRadius = 0.5f;
    public int maxAttempts = 100;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
            // このオブジェクトのみを削除
            Destroy(gameObject);
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                spawnedEnemies.Add(enemy);
            }
            else
            {
                Debug.LogWarning("Could not find a valid spawn position for enemy " + i);
            }
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                0,
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );

            randomPosition += transform.position;

            if (!Physics.CheckBox(randomPosition, Vector3.one * checkRadius, Quaternion.identity, LayerMask.GetMask("Enemy")))
            {
                return randomPosition;
            }
        }

        return Vector3.zero;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
