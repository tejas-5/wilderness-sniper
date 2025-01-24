using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmController : MonoBehaviour
{
    [SerializeField] int armHp = 10;
    [SerializeField] int scoreValue = 100;
    [SerializeField] AudioClip destructionSound;
    private ScoreManager scoreManager;
    private AudioSource audioSource; 
    private bool isDestroyed = false; 

    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager")?.GetComponent<ScoreManager>();
        audioSource = GetComponent<AudioSource>();

        if (scoreManager == null)
        {
            Debug.LogWarning("ScoreManagerが見つかりませんでした。");
        }
    }

    void Update()
    {
        if (armHp == 0 && !isDestroyed) 
        {
            Die();
        }
    }

    void OnMouseDown()
    {
        if (GameManager.Instance.AnyScreenEnabled())
        {
            return;
        }
        // armHp を減らす
        armHp = Mathf.Max(armHp - 1, 0);
    }

    private void Die()
    {
        isDestroyed = true;

        if (scoreManager != null)
        {
            scoreManager.AddScore(scoreValue);
        }
        else
        {
            Debug.LogWarning("スコアを加算できません。ScoreManagerが設定されていません。");
        }

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
}
