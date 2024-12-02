using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController: MonoBehaviour
{
    private Vector2 pos;
    [SerializeField] int num = 1;//方向

    [SerializeField] GameObject bossObject; 
    [SerializeField] Vector3 startPosition; // 初期位置
    [SerializeField] Vector3 endPosition;    // 移動後の位置
    [SerializeField] float spawnDelay = 60f;// スポーンまでの時間
    [SerializeField] float moveDuration = 0.3f;// 移動にかかる時間

    [SerializeField] GameObject missilePrefab; 
    [SerializeField] float spawnInterval = 3f; // オブジェクトを生成する間隔（秒）
    private bool isSpawning = true; // 生成を制御するフラグ
    [SerializeField] float missileDelay = 62f;
    [SerializeField] Transform rightArm;
    [SerializeField] Transform leftArm;

    void Start()
    {
        transform.position = startPosition;

        if (bossObject != null) StartCoroutine(SpawnBoss());
        
        if (bossObject != null) StartCoroutine(Missile());
    }

    void Update()
    {
        pos = transform.position;

        // （ポイント）マイナスをかけることで逆方向に移動する。
        transform.Translate(transform.right * Time.deltaTime * 3 * num);

        if (pos.x > 5.5) num = -1;
        if (pos.x < -5.5) num = 1;
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

    private IEnumerator Missile()
    {
        yield return new WaitForSeconds(missileDelay);

        while (isSpawning)
        {
            // オブジェクトを生成
            SpawnMissile();

            // 一定時間待機
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    void SpawnMissile()
    {
        if (missilePrefab != null &&
            rightArm != null　&&
            leftArm != null)
        {
            // 発射位置をランダムで選択
            Transform selectedPosition = Random.Range(0, 2) == 0 ? rightArm : leftArm;

            // 指定された位置と回転でオブジェクトを生成
            Instantiate(missilePrefab, selectedPosition.position, selectedPosition.rotation);
        }
    }
}
