using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeySpawn : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab; // 敵のプレハブ
    [SerializeField] float spawnInterval; // スポーン間隔
    [SerializeField] List<Vector3> spawnPoints; // スポーン位置のリスト
    [SerializeField] float newSpawnInterval; // 60秒後に設定する新しいスポーン間隔

    private int lastSpawnIndex = -1; // 前回のスポーン位置のインデックス

    private void Start()
    {
        // 一定の間隔でSpawnEnemyメソッドを呼び出す
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);

        StartCoroutine(ChangeSpawnIntervalAfterDelay(60f));
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Count == 0)
        {
            return;
        }

        int spawnIndex;
        do
        {
            spawnIndex = Random.Range(0, spawnPoints.Count);
        } while (spawnIndex == lastSpawnIndex && spawnPoints.Count > 1);

        lastSpawnIndex = spawnIndex; // 現在のインデックスを保存

        Vector3 spawnPosition = spawnPoints[spawnIndex];
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private IEnumerator ChangeSpawnIntervalAfterDelay(float delay)
    {
        // 指定時間（60秒）待機
        yield return new WaitForSeconds(delay);

        // 新しいスポーン間隔を設定
        CancelInvoke("SpawnEnemy"); // 古い間隔での呼び出しを停止
        spawnInterval = newSpawnInterval;
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval); // 新しい間隔で再設定
    }
}