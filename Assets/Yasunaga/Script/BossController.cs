using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController: MonoBehaviour
{
    private Vector2 pos;
    [SerializeField] int num = 1;

    [SerializeField] GameObject bossObject; // 有効化する対象のオブジェクト
    [SerializeField] Vector3 startPosition; // 初期位置
    [SerializeField] Vector3 endPosition;    // 移動後の位置
    [SerializeField] float spawnDelay;// スポーンまでの時間
    [SerializeField] float moveDuration;// 移動にかかる時間

    [SerializeField] GameObject missilePrefab; // 生成するオブジェクトのプレハブ
    [SerializeField] Transform spawnPoint;  // 生成位置（ボスの前や周囲など）
    [SerializeField] float spawnInterval; // オブジェクトを生成する間隔（秒）
    private bool isSpawning = true; // 生成を制御するフラグ
    [SerializeField] float missileDelay;

    void Start()
    {
        transform.position = startPosition;

        if (bossObject != null) StartCoroutine(SpawnBoss());
        
        if (bossObject != null) StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        pos = transform.position;

        // （ポイント）マイナスをかけることで逆方向に移動する。
        transform.Translate(transform.right * Time.deltaTime * 3 * num);

        if (pos.x > 5.5)
        {
            num = -1;
        }
        if (pos.x < -5.5)
        {
            num = 1;
        }
    }

    private IEnumerator SpawnBoss()
    {
        // 1分待機
        yield return new WaitForSeconds(spawnDelay);

        bossObject.SetActive(true);

        // 移動
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // フレームをまたぐ
        }

        // 最終位置を確実に設定
        transform.position = endPosition;
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(missileDelay);

        while (isSpawning)
        {
            // オブジェクトを生成
            SpawnObject();

            // 一定時間待機
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    void SpawnObject()
    {
        if (missilePrefab != null && spawnPoint != null)
        {
            // 指定された位置でオブジェクトを生成
            Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
