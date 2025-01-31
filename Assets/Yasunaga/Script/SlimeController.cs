using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField] float scaleSpeed = 0.3f; // �X�P�[���̑��x
    private Vector3 initialScale;    // �����X�P�[��
    [SerializeField] float maxSize = 2.0f;     // �ő�T�C�Y
    [SerializeField] float flashSize = 1.7f;     // �ő�T�C�Y

    [SerializeField] int scoreValue = 10; // ���̓G��|�������̃X�R�A
    private ScoreManager scoreManager;
    [SerializeField] int damage = 10;
    private PlayerController playerController;

    private Renderer objectRenderer; // �I�u�W�F�N�g��Renderer
    private bool isFlashing = false; // �_�Œ����ǂ���

    [SerializeField] AudioClip destructionSound;
    private AudioSource audioSource;

    void Start()
    {
        initialScale = transform.localScale; // �����X�P�[�����擾

        // �X�R�A�}�l�[�W���[�̃I�u�W�F�N�g���擾
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // Renderer�R���|�[�l���g���擾
        objectRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // �X�P�[�������X�ɑ傫������
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
        Color originalColor = objectRenderer.material.color; // ���̐F��ۑ�
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
