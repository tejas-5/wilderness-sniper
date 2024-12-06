using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmController : MonoBehaviour
{
    [SerializeField] int armHp = 10;
    [SerializeField] int hitDamage = 1;
    public int scoreValue = 100; // ‚±‚Ì“G‚ğ“|‚µ‚½‚ÌƒXƒRƒA
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

    public void Die()
    {
        // ƒXƒRƒA‚ğ‰ÁZ
        scoreManager.AddScore(scoreValue);

        // “G‚ğíœ
        Destroy(gameObject);
    }
}
