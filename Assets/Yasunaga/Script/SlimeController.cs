using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField] float scaleSpeed = 0.3f; // スケールの速度
    private Vector3 initialScale;    // 初期スケール
    [SerializeField] float maxSize = 2.0f;     // 最大サイズ
    [SerializeField] float flashSize = 1.7f;     // 最大サイズ

    [SerializeField] int scoreValue = 10; // この敵を倒した時のスコア
    private ScoreManager scoreManager;
    [SerializeField] int damage = 10;
    private PlayerController playerController;

    private Renderer objectRenderer; // オブジェクトのRenderer
    private bool isFlashing = false; // 点滅中かどうか

    [SerializeField] AudioClip destructionSound;
    private AudioSource audioSource;

    void Start()
    {
        initialScale = transform.localScale; // 初期スケールを取得

        // スコアマネージャーのオブジェクトを取得
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // Rendererコンポーネントを取得
        objectRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // スケールを徐々に大きくする
        transform.localScale += initialScale * scaleSpeed * Time.deltaTime;

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

        if (audioSource != null && destructionSound != null)
        {
            audioSource.PlayOneShot(destructionSound);
            Destroy(gameObject, destructionSound.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Damage()
    {
        playerController.AddDamage(damage);
        Destroy(gameObject);
    }

    private IEnumerator Flash()
    {
        isFlashing = true;
        Color originalColor = objectRenderer.material.color; // 元の色を保存
        Color flashColor = new Color(1f, 0.5f, 0f, 1f);
        while (true)
        {
            objectRenderer.material.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            objectRenderer.material.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
