using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab; // 敵のプレハブ
    [SerializeField] float spawnInterval; // スポーン間隔
    [SerializeField] List<Vector3> spawnPoints; // スポーン位置のリスト
    [SerializeField] float newSpawnInterval; // 60秒後に設定する新しいスポーン間隔

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

        // ランダムにスポーン位置を選択
        Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // 敵をスポーン
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