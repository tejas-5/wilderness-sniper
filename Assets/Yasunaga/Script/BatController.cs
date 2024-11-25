using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    [SerializeField] float xRadius = 3.0f;  // X方向の半径
    [SerializeField] float yRadius = 1.0f;  // Y方向の半径
    private float duration = 1.25f; // 楕円の片道移動にかける時間（秒）
    private Vector2 centerPosition; // 楕円の中心位置

    [SerializeField] float scaleSpeed = 0.3f; // スケールの速度
    private Vector3 firstScale;    // 初期スケール
    [SerializeField] float maxSize = 2.0f;     // 最大サイズ

    public int scoreValue = 50; // この敵を倒した時のスコア
    private ScoreManager scoreManager;
    public int damage = 10;
    private PlayerController playerController;

    void Start()
    {
        // オブジェクトの初期位置を中心として設定
        centerPosition = transform.position;

        // コルーチン開始
        StartCoroutine(MoveBat());

        firstScale = transform.localScale; // 初期スケールを取得

        // スコアマネージャーのオブジェクトを取得
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

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

    private void Update()
    {
        // スケールを徐々に大きくする
        transform.localScale += firstScale * scaleSpeed * Time.deltaTime;

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

    public void Die()
    {
        // スコアを加算
        scoreManager.AddScore(scoreValue);

        // 敵を削除
        Destroy(gameObject);
    }

    public void Damage()
    {
        playerController.AddDamage(damage);
        Destroy(gameObject); // オブジェクトを消去
    }
}
