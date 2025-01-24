using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    //��s�ړ�
    private float xRadius = 3.0f;  // X�����̔��a
    private float yRadius = 1.0f;  // Y�����̔��a
    private float duration = 1.25f; // �ȉ~�̕Г��ړ��ɂ����鎞�ԁi�b�j
    private Vector2 centerPosition; // �ȉ~�̒��S�ʒu

    //�߂Â�
    [SerializeField] float scaleSpeed = 0.3f; // �X�P�[���̑��x
    private Vector3 firstScale;    // �����X�P�[��
    [SerializeField] float maxSize = 2.0f;     // �ő�T�C�Y
    [SerializeField] float flashSize = 1.7f;     // �ő�T�C�Y

    //�p�����[�^�[
    private ScoreManager scoreManager;
    [SerializeField] int scoreValue = 50; // ���̓G��|�������̃X�R�A
    [SerializeField] int damage = 10; //�󂯂�_���[�W
    private PlayerController playerController;

    private Renderer objectRenderer; // �I�u�W�F�N�g��Renderer
    private bool isFlashing = false; // �_�Œ����ǂ���

    [SerializeField] private GameObject panel;

    void Start()
    {
        centerPosition = transform.position;
        firstScale = transform.localScale;

        StartCoroutine(MoveBat());

        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // Renderer�R���|�[�l���g���擾
        objectRenderer = GetComponent<Renderer>();
    }
    //��s�ړ�
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
    //�ȉ~�ړ�
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
    //�߂Â�
    private void Update()
    {
        // �X�P�[�������X�ɑ傫������
        transform.localScale += firstScale * scaleSpeed * Time.deltaTime;


        if (transform.localScale.x >= flashSize ||
        transform.localScale.y >= flashSize ||
        transform.localScale.z >= flashSize)
        {
            if (!isFlashing)
            {
                StartCoroutine(Flash());
            }
        }
        // ���݂̃T�C�Y���ő�T�C�Y�𒴂�����I�u�W�F�N�g������
        if (transform.localScale.x >= maxSize ||
            transform.localScale.y >= maxSize ||
            transform.localScale.z >= maxSize)
        {
            Damage();
        }
    }

    private IEnumerator Flash()
    {
        isFlashing = true;
        Color originalColor = objectRenderer.material.color; // ���̐F��ۑ�
        Color flashColor = new Color(1f, 0f, 0f, 1f);
        while (true)
        {
            objectRenderer.material.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            objectRenderer.material.color = originalColor;
            yield return new WaitForSeconds(0.1f);
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
