using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField] float scaleSpeed = 0.3f; // �X�P�[���̑��x
    private Vector3 initialScale;    // �����X�P�[��
    [SerializeField] float maxSize = 2.0f;     // �ő�T�C�Y

    [SerializeField] int scoreValue = 10; // ���̓G��|�������̃X�R�A
    private ScoreManager scoreManager;
    [SerializeField] int damage = 10;
    private PlayerController playerController;

   

    void Start()
    {
        initialScale = transform.localScale; // �����X�P�[�����擾

        // �X�R�A�}�l�[�W���[�̃I�u�W�F�N�g���擾
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        
    }

    void Update()
    {
        // �X�P�[�������X�ɑ傫������
        transform.localScale += initialScale * scaleSpeed * Time.deltaTime;

        // ���݂̃T�C�Y���ő�T�C�Y�𒴂�����I�u�W�F�N�g������
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
        // �X�R�A�����Z
        scoreManager.AddScore(scoreValue);

        // �G���폜
        Destroy(gameObject);
    }

    private void Damage()
    {
        playerController.AddDamage(damage);
        Destroy(gameObject); // �I�u�W�F�N�g������
    }
}
