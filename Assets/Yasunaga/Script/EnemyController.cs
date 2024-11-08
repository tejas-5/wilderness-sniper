using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float scaleSpeed = 0.3f; // �X�P�[���̑��x
    private Vector3 initialScale;    // �����X�P�[��
    public float maxSize = 2.0f;     // �ő�T�C�Y

    void Start()
    {
        initialScale = transform.localScale; // �����X�P�[�����擾
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
            Destroy(gameObject); // �I�u�W�F�N�g������
        }

    }

    void OnMouseDown()
    {
        // ���̃I�u�W�F�N�g�݂̂��폜
        Destroy(gameObject);
    }
}
