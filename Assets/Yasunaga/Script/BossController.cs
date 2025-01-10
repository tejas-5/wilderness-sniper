using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    //ボス移動
    private Vector2 pos;
    private int num = 1;//方向
    [SerializeField] int moveSpeed = 3;

    //ボススポーン
    [SerializeField] float spawnDelay = 60f;// スポーンまでの時間
    [SerializeField] Vector3 startPosition; // 初期位置
    [SerializeField] Vector3 endPosition;    // 最終位置
    [SerializeField] float moveDuration = 0.3f; //最終位置移動までかかる時間

    //ハサミからの攻撃
    private bool isArmAttack = true; // 生成を制御するフラグ
    [SerializeField] float missileDelay = 62f;//ミサイル発射までの時間
    [SerializeField] GameObject missilePrefab;
    [SerializeField] float armInterval = 3f; // オブジェクトを生成する間隔（秒）
    [SerializeField] Transform rightArm;
    [SerializeField] Transform leftArm;
    //尻尾からの攻撃
    private bool isTealAttack = false;
    [SerializeField] GameObject fastMissilePrefab;
    [SerializeField] float tealInterval = 2f;
    [SerializeField] Transform teal;

    //パラメーター
    [SerializeField] int bossHp = 20;
    [SerializeField] int hitDamage = 1;
    [SerializeField] int scoreValue = 150; // この敵を倒した時のスコア
    private ScoreManager scoreManager;

    void Start()
    {
        transform.position = startPosition;

        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        StartCoroutine(SpawnBoss());

        StartCoroutine(ArmAttack());
    }

    void Update()
    {
        pos = transform.position;

        // マイナスをかけることで逆方向に移動
        transform.Translate(transform.right * Time.deltaTime * moveSpeed * num);
        if (pos.x > 5.5) num = -1;
        if (pos.x < -5.5) num = 1;

        if (bossHp == 0) Die();
    }

    //ボススポーン
    private IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(spawnDelay);

        // 最終位置まで移動
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;

            yield return null;

            yield return null; 

        }
        //最終位置を設定
        transform.position = endPosition;
    }

    //ハサミ攻撃
    private IEnumerator ArmAttack()
    {
        yield return new WaitForSeconds(missileDelay);

        while (isArmAttack)
        {
            ArmMissile();
            yield return new WaitForSeconds(armInterval);
        }
    }
    //ハサミミサイル
    void ArmMissile()
    {
        if (rightArm != null || leftArm != null)
        {
            Transform selectedPosition = null;

            // 発射位置をランダムで選択
            if (rightArm != null && leftArm != null) 
                selectedPosition = Random.Range(0, 2) == 0 ? rightArm : leftArm;
            // 右ハサミのみ存在する場合
            else if (rightArm != null) 
                selectedPosition = rightArm;
            // 左ハサミのみ存在する場合
            else if (leftArm != null) 
                selectedPosition = leftArm;

            // 発射位置が決定している場合のみミサイルを生成
            if (selectedPosition != null) 
                Instantiate(missilePrefab, selectedPosition.position, selectedPosition.rotation);
        }
        //両ハサミが破壊されたら尻尾攻撃
        else if (!isTealAttack)
        {
            isTealAttack = true; // 尻尾攻撃開始フラグを設定
            StartCoroutine(TealAttack());
        }
    }

    //尻尾攻撃
    private IEnumerator TealAttack()
    {

        while (isTealAttack)
        {
            TealMissile();
            yield return new WaitForSeconds(tealInterval);

            while (isTealAttack)
            {
                TealMissile();
                yield return new WaitForSeconds(tealInterval);

            }
        }
        //尻尾ミサイル
        void TealMissile()
        {
            if (teal != null)
                Instantiate(fastMissilePrefab, teal.position, teal.rotation);
        }   
    }
    void Die()
    {
        // スコアを加算
        scoreManager.AddScore(scoreValue);

        Destroy(gameObject);
    }
}