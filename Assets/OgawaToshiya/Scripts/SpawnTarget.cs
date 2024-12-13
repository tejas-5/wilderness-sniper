using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTarget : MonoBehaviour
{
    // Prefab
    public GameObject TargerPrefab;
    public Vector3 spawnPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (TargerPrefab != null)
        {
            Instantiate(TargerPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
