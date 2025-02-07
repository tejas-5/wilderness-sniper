using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastMissile : MonoBehaviour
{
    [SerializeField] float scaleSpeed = 0.5f; // スケールの速度
    private Vector3 initialScale;    // 初期スケール
    [SerializeField] float maxSize = 2.5f;     // 最大サイズ

    [SerializeField] int scoreValue = 50; // この敵を倒した時のスコア
    private ScoreManager scoreManager;
    [SerializeField] int damage = 10;
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
        // スケールを徐々に大きくする
        transform.localScale += initialScale * scaleSpeed * Time.deltaTime;

        transform.Translate(0, -0.003f, 0);

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
        Destroy(gameObject); 
    }
}
