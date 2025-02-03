using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField] float moveSpeed; // 上下移動の速度
    [SerializeField] float targetY;  // 到達するY地点
    [SerializeField] float fallSpeed; // -Y方向に落ちる速度

    private bool movingUp = true; // 上昇中か下降中かを判定

    [SerializeField] float scaleSpeed = 0.2f; // スケールの速度
    private Vector3 initialScale;    // 初期スケール
    [SerializeField] float maxSize = 2.3f;     // 最大サイズ

    [SerializeField] int scoreValue = 50; // この敵を倒した時のスコア
    private ScoreManager scoreManager;
    [SerializeField] int damage = 10; //受けるダメージ
    private PlayerController playerController;
    private Animator animator;

    void Start()
    {
        initialScale = transform.localScale; // 初期スケールを取得

        // スコアマネージャーのオブジェクトを取得
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        MoveObject();
    }
    void MoveObject()
    {
        // 現在の位置
        Vector3 currentPosition = transform.position;

        if (movingUp)
        {
            // Y方向に上昇
            currentPosition.y += moveSpeed * Time.deltaTime;

            // 設定したY地点に到達したら反転
            if (currentPosition.y >= targetY)
            {
                movingUp = false; // 落下に切り替え
                transform.Rotate(0f, 0f, 180f);

                
            }
        }
        else
        {
            // Y方向に下降
            currentPosition.y -= fallSpeed * Time.deltaTime;

            // 必要であれば、下降中の制御を追加（地面に到達時など）
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

        // 現在の位置を更新
        transform.position = currentPosition;
    }

    void OnMouseDown()
    {
        if (GameManager.Instance.AnyScreenEnabled())
        {
            return;
        }
        Die();
    }

    private void Die()
    {
        // スコアを加算
        scoreManager.AddScore(scoreValue);

        animator.SetTrigger("Effect");
        StartCoroutine(Destroy());
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    private void Damage()
    {
        playerController.AddDamage(damage);
        Destroy(gameObject); // オブジェクトを消去
    }
}
