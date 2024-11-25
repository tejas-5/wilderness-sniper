using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab; // �G�̃v���n�u
    [SerializeField] float spawnInterval; // �X�|�[���Ԋu
    [SerializeField] List<Vector3> spawnPoints; // �X�|�[���ʒu�̃��X�g
    [SerializeField] float newSpawnInterval; // 60�b��ɐݒ肷��V�����X�|�[���Ԋu

    private void Start()
    {
        // ���̊Ԋu��SpawnEnemy���\�b�h���Ăяo��
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);

        StartCoroutine(ChangeSpawnIntervalAfterDelay(60f));
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Count == 0)
        {
            return;
        }

        // �����_���ɃX�|�[���ʒu��I��
        Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // �G���X�|�[��
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private IEnumerator ChangeSpawnIntervalAfterDelay(float delay)
    {
        // �w�莞�ԁi60�b�j�ҋ@
        yield return new WaitForSeconds(delay);

        // �V�����X�|�[���Ԋu��ݒ�
        CancelInvoke("SpawnEnemy"); // �Â��Ԋu�ł̌Ăяo�����~
        spawnInterval = newSpawnInterval;
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval); // �V�����Ԋu�ōĐݒ�
    }
}