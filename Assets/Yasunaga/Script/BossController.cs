using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController: MonoBehaviour
{
    private Vector2 pos;
    [SerializeField] int num = 1;//方向

    [SerializeField] Vector3 startPosition; // 初期位置
    [SerializeField] Vector3 endPosition;    // 移動後の位置
    [SerializeField] float spawnDelay = 60f;// スポーンまでの時間
    [SerializeField] float moveDuration = 0.3f;// 移動にかかる時間

    [SerializeField] GameObject missilePrefab; 
    [SerializeField] GameObject fastMissilePrefab;
    [SerializeField] float spawnInterval = 3f; // オブジェクトを生成する間隔（秒）
    private bool isSpawning = true; // 生成を制御するフラグ
    [SerializeField] float missileDelay = 62f;
    [SerializeField] Transform rightArm;
    [SerializeField] Transform leftArm;
    [SerializeField] Transform teal;

    [SerializeField] int bossHp = 15;
    [SerializeField] int hitDamage = 1;
    [SerializeField] int scoreValue = 150; // この敵を倒した時のスコア
    private ScoreManager scoreManager;

    void Start()
    {
        transform.position = startPosition;

        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        StartCoroutine(SpawnBoss());
        
        StartCoroutine(Missile());

    }

    void Update()
    {
        pos = transform.position;

        // （ポイント）マイナスをかけることで逆方向に移動する。
        transform.Translate(transform.right * Time.deltaTime * 3 * num);

        if (pos.x > 5.5) num = -1;
        if (pos.x < -5.5) num = 1;
        Debug.Log(bossHp);

        if (bossHp == 0)
        {
            Die();
        }
    }

    private IEnumerator SpawnBoss()
    {
        // 1分待機
        yield return new WaitForSeconds(spawnDelay);

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
        if (rightArm != null || leftArm != null)
        {
            Transform selectedPosition = null;

            // 発射位置をランダムで選択（存在するもののみ）
            if (rightArm != null && leftArm != null)
            {
                // 両方存在する場合はランダムに選択
                selectedPosition = Random.Range(0, 2) == 0 ? rightArm : leftArm;
            }
            else if (rightArm != null)
            {
                // 右アームのみ存在する場合
                selectedPosition = rightArm;
            }
            else if (leftArm != null)
            {
                // 左アームのみ存在する場合
                selectedPosition = leftArm;
            }

            // 発射位置が決定している場合のみミサイルを生成
            if (selectedPosition != null)
            {
                Instantiate(missilePrefab, selectedPosition.position, selectedPosition.rotation);
            }
        }
        else
        {
            // 両アームが存在しない場合のイベント
            HandleNoArms();
        }
    }

    void HandleNoArms()
    {
        StartCoroutine(FastMissile());
    }
    private IEnumerator FastMissile()
    {
        while (true) // 必要に応じて条件を追加して終了タイミングを制御
        {
            // 特殊攻撃としてオブジェクトを生成
            SpawnSpecialObject();

            // 一定時間待機
            yield return new WaitForSeconds(spawnInterval); // spawnIntervalを流用
        }
    }
    void SpawnSpecialObject()
    {
        if (teal != null)
        {
            Transform selectedPosition = null;
            if (selectedPosition != null)
            {
                Instantiate(fastMissilePrefab, selectedPosition.position, selectedPosition.rotation);
            }
        }
    }
    void OnMouseDown()
    {
        bossHp -= hitDamage;
    }

    public void Die()
    {
        // スコアを加算
        scoreManager.AddScore(scoreValue);

        // 敵を削除
        Destroy(gameObject);
    }
}
