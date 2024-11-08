using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public GameObject enemyPrefab; // �G�̃v���n�u
    public float spawnInterval = 3f; // �X�|�[���Ԋu
    public List<Vector3> spawnPoints; // �X�|�[���ʒu�̃��X�g

    private void Start()
    {
        // ���̊Ԋu��SpawnEnemy���\�b�h���Ăяo��
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        // �����_���ɃX�|�[���ʒu��I��
        Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // �G���X�|�[��
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}