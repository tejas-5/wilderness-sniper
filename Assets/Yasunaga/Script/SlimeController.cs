using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float scaleSpeed = 0.3f; // スケールの速度
    private Vector3 initialScale;    // 初期スケール
    public float maxSize = 2.0f;     // 最大サイズ

    public int scoreValue = 10; // この敵を倒した時のスコア
    private ScoreManager scoreManager;

    void Start()
    {
        initialScale = transform.localScale; // 初期スケールを取得

        // スコアマネージャーのオブジェクトを取得
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
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
            Destroy(gameObject); // オブジェクトを消去
        }

    }

    void OnMouseDown()
    {
        Die();
    }

    public void Die()
    {
        // スコアを加算
        scoreManager.AddScore(scoreValue);

        // 敵を削除
        Destroy(gameObject);
    }
}
