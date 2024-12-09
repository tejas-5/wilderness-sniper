using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField] float scaleSpeed = 0.3f; // スケールの速度
    private Vector3 initialScale;    // 初期スケール
    [SerializeField] float maxSize = 2.0f;     // 最大サイズ

    [SerializeField] int scoreValue = 10; // この敵を倒した時のスコア
    private ScoreManager scoreManager;
    [SerializeField] int damage = 10;
    private PlayerController playerController;

   

    void Start()
    {
        initialScale = transform.localScale; // 初期スケールを取得

        // スコアマネージャーのオブジェクトを取得
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        
    }

    void Update()
    {
        // スケールを徐々に大きくする
        transform.localScale += initialScale * scaleSpeed * Time.deltaTime;

        // 現在のサイズが最大サイズを超えたらオブジェクトを消去
        if (transform.localScale.x >= maxSize ||
            transform.localScale.y >= maxSize ||
            transform.localScale.z >= maxSize)
        {
            Damage();
        }

    }

    void OnMouseDown()
    {
        Die();
    }

    private void Die()
    {
        // スコアを加算
        scoreManager.AddScore(scoreValue);

        // 敵を削除
        Destroy(gameObject);
    }

    private void Damage()
    {
        playerController.AddDamage(damage);
        Destroy(gameObject); // オブジェクトを消去
    }
}
