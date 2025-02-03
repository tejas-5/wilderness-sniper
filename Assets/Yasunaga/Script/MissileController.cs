using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField] float moveSpeed; // �㉺�ړ��̑��x
    [SerializeField] float targetY;  // ���B����Y�n�_
    [SerializeField] float fallSpeed; // -Y�����ɗ����鑬�x

    private bool movingUp = true; // �㏸�������~�����𔻒�

    [SerializeField] float scaleSpeed = 0.2f; // �X�P�[���̑��x
    private Vector3 initialScale;    // �����X�P�[��
    [SerializeField] float maxSize = 2.3f;     // �ő�T�C�Y

    [SerializeField] int scoreValue = 50; // ���̓G��|�������̃X�R�A
    private ScoreManager scoreManager;
    [SerializeField] int damage = 10; //�󂯂�_���[�W
    private PlayerController playerController;
    private Animator animator;

    void Start()
    {
        initialScale = transform.localScale; // �����X�P�[�����擾

        // �X�R�A�}�l�[�W���[�̃I�u�W�F�N�g���擾
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
        // ���݂̈ʒu
        Vector3 currentPosition = transform.position;

        if (movingUp)
        {
            // Y�����ɏ㏸
            currentPosition.y += moveSpeed * Time.deltaTime;

            // �ݒ肵��Y�n�_�ɓ��B�����甽�]
            if (currentPosition.y >= targetY)
            {
                movingUp = false; // �����ɐ؂�ւ�
                transform.Rotate(0f, 0f, 180f);

                
            }
        }
        else
        {
            // Y�����ɉ��~
            currentPosition.y -= fallSpeed * Time.deltaTime;

            // �K�v�ł���΁A���~���̐����ǉ��i�n�ʂɓ��B���Ȃǁj
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

        // ���݂̈ʒu���X�V
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
        // �X�R�A�����Z
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
        Destroy(gameObject); // �I�u�W�F�N�g������
    }
}
