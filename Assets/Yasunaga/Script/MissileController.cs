using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField] float moveSpeed; // �㉺�ړ��̑��x
    [SerializeField] float targetY;  // ���B����Y�n�_
    [SerializeField] float fallSpeed; // -Y�����ɗ����鑬�x

    private bool movingUp = true; // �㏸�������~�����𔻒�

    [SerializeField] float scaleSpeed = 0.3f; // �X�P�[���̑��x
    private Vector3 initialScale;    // �����X�P�[��
    [SerializeField] float maxSize = 2.0f;     // �ő�T�C�Y

    public int scoreValue = 50; // ���̓G��|�������̃X�R�A
    private ScoreManager scoreManager;

    void Start()
    {
        initialScale = transform.localScale; // �����X�P�[�����擾

        // �X�R�A�}�l�[�W���[�̃I�u�W�F�N�g���擾
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
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
                Destroy(gameObject); // �I�u�W�F�N�g������
            }
        }

        // ���݂̈ʒu���X�V
        transform.position = currentPosition;
    }

    void OnMouseDown()
    {
        Die();
    }

    public void Die()
    {
        // �X�R�A�����Z
        scoreManager.AddScore(scoreValue);

        // �G���폜
        Destroy(gameObject);
    }
}