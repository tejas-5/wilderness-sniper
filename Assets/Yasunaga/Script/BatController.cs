using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    //飛行移動
    private float xRadius = 3.0f;  // X方向の半径
    private float yRadius = 1.0f;  // Y方向の半径
    private float duration = 1.25f; // 楕円の片道移動にかける時間（秒）
    private Vector2 centerPosition; // 楕円の中心位置

    //近づく
    [SerializeField] float scaleSpeed = 0.3f; // スケールの速度
    private Vector3 firstScale;    // 初期スケール
    [SerializeField] float maxSize = 2.0f;     // 最大サイズ
    [SerializeField] float flashSize = 1.7f;     // 最大サイズ

    //パラメーター
    private ScoreManager scoreManager;
    [SerializeField] int scoreValue = 50; // この敵を倒した時のスコア
    [SerializeField] int damage = 10; //受けるダメージ
    private PlayerController playerController;

    private Renderer objectRenderer; // オブジェクトのRenderer
    private bool isFlashing = false; // 点滅中かどうか

    [SerializeField] private GameObject panel;

    void Start()
    {
        centerPosition = transform.position;
        firstScale = transform.localScale;

        StartCoroutine(MoveBat());

        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // Rendererコンポーネントを取得
        objectRenderer = GetComponent<Renderer>();
    }
    //飛行移動
    IEnumerator MoveBat()
    {
        while (true)
        {
            // 順方向の楕円を描く
            yield return StartCoroutine(EllipseMotion(0.0f, 180.0f));
            // 逆方向の楕円を描く（元の位置に戻る）
            yield return StartCoroutine(EllipseMotion(180.0f, 0.0f));
        }
    }
    //楕円移動
    IEnumerator EllipseMotion(float startAngle, float endAngle)
    {
        float angle = startAngle;
        float angleIncrement = Mathf.Abs(endAngle - startAngle) / duration;

        while (Mathf.Abs(angle - endAngle) > 0.1f) // endAngleに近づくまで
        {
            // 角度の更新（startAngleからendAngleの方向に進む）
            angle = Mathf.MoveTowards(angle, endAngle, angleIncrement * Time.deltaTime);

            // XとYの半径と角度を使って楕円の位置を計算
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * xRadius;
            float y = Mathf.Sin(-angle * Mathf.Deg2Rad) * yRadius;

            // 新しい位置を設定（中心位置を基準に移動）
            transform.position = centerPosition + new Vector2(x, y);

            // 次のフレームまで待機
            yield return null;
        }
    }
    //近づく
    private void Update()
    {
        // スケールを徐々に大きくする
        transform.localScale += firstScale * scaleSpeed * Time.deltaTime;


        if (transform.localScale.x >= flashSize ||
        transform.localScale.y >= flashSize ||
        transform.localScale.z >= flashSize)
        {
            if (!isFlashing)
            {
                StartCoroutine(Flash());
            }
        }
        // 現在のサイズが最大サイズを超えたらオブジェクトを消去
        if (transform.localScale.x >= maxSize ||
            transform.localScale.y >= maxSize ||
            transform.localScale.z >= maxSize)
        {
            Damage();
        }
    }

    private IEnumerator Flash()
    {
        isFlashing = true;
        Color originalColor = objectRenderer.material.color; // 元の色を保存
        Color flashColor = new Color(1f, 0f, 0f, 1f);
        while (true)
        {
            objectRenderer.material.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            objectRenderer.material.color = originalColor;
            yield return new WaitForSeconds(0.1f);
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

        // 敵を削除
        Destroy(gameObject);
    }

    private void Damage()
    {
        playerController.AddDamage(damage);
        Destroy(gameObject); // オブジェクトを消去
    }
}
