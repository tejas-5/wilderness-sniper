using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmController : MonoBehaviour
{
    [SerializeField] int armHp = 10;
    [SerializeField] int hitDamage = 1;
    [SerializeField] int scoreValue = 100; // この敵を倒した時のスコア
    private ScoreManager scoreManager;
    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }
    void Update()
    {
        if (armHp == 0)
        {
            Die();
        }
        Debug.Log(armHp);
    }
    void OnMouseDown()
    {
        armHp -= hitDamage;
    }
    private void Die()
    {
        // スコアを加算
        scoreManager.AddScore(scoreValue);

        // 敵を削除
        Destroy(gameObject);
    }
}
