using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public GameObject enemyPrefab; // 敵のプレハブ
    public float spawnInterval = 3f; // スポーン間隔
    public List<Vector3> spawnPoints; // スポーン位置のリスト

    private void Start()
    {
        // 一定の間隔でSpawnEnemyメソッドを呼び出す
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        // ランダムにスポーン位置を選択
        Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // 敵をスポーン
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}