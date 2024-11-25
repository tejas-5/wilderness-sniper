using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    [SerializeField] float xRadius = 3.0f;  // X�����̔��a
    [SerializeField] float yRadius = 1.0f;  // Y�����̔��a
    private float duration = 1.25f; // �ȉ~�̕Г��ړ��ɂ����鎞�ԁi�b�j
    private Vector2 centerPosition; // �ȉ~�̒��S�ʒu

    [SerializeField] float scaleSpeed = 0.3f; // �X�P�[���̑��x
    private Vector3 firstScale;    // �����X�P�[��
    [SerializeField] float maxSize = 2.0f;     // �ő�T�C�Y

    public int scoreValue = 50; // ���̓G��|�������̃X�R�A
    private ScoreManager scoreManager;
    public int damage = 10;
    private PlayerController playerController;

    void Start()
    {
        // �I�u�W�F�N�g�̏����ʒu�𒆐S�Ƃ��Đݒ�
        centerPosition = transform.position;

        // �R���[�`���J�n
        StartCoroutine(MoveBat());

        firstScale = transform.localScale; // �����X�P�[�����擾

        // �X�R�A�}�l�[�W���[�̃I�u�W�F�N�g���擾
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    IEnumerator MoveBat()
    {
        while (true)
        {
            // �������̑ȉ~��`��
            yield return StartCoroutine(EllipseMotion(0.0f, 180.0f));

            // �t�����̑ȉ~��`���i���̈ʒu�ɖ߂�j
            yield return StartCoroutine(EllipseMotion(180.0f, 0.0f));
        }
    }

    IEnumerator EllipseMotion(float startAngle, float endAngle)
    {
        float angle = startAngle;
        float angleIncrement = Mathf.Abs(endAngle - startAngle) / duration;

        while (Mathf.Abs(angle - endAngle) > 0.1f) // endAngle�ɋ߂Â��܂�
        {
            // �p�x�̍X�V�istartAngle����endAngle�̕����ɐi�ށj
            angle = Mathf.MoveTowards(angle, endAngle, angleIncrement * Time.deltaTime);

            // X��Y�̔��a�Ɗp�x���g���đȉ~�̈ʒu���v�Z
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * xRadius;
            float y = Mathf.Sin(-angle * Mathf.Deg2Rad) * yRadius;

            // �V�����ʒu��ݒ�i���S�ʒu����Ɉړ��j
            transform.position = centerPosition + new Vector2(x, y);

            // ���̃t���[���܂őҋ@
            yield return null;
        }
    }

    private void Update()
    {
        // �X�P�[�������X�ɑ傫������
        transform.localScale += firstScale * scaleSpeed * Time.deltaTime;

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

    public void Die()
    {
        // �X�R�A�����Z
        scoreManager.AddScore(scoreValue);

        // �G���폜
        Destroy(gameObject);
    }

    public void Damage()
    {
        playerController.AddDamage(damage);
        Destroy(gameObject); // �I�u�W�F�N�g������
    }
}
