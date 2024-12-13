using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController: MonoBehaviour
{
    private Vector2 pos;
    [SerializeField] int num = 1;//����

    [SerializeField] Vector3 startPosition; // �����ʒu
    [SerializeField] Vector3 endPosition;    // �ړ���̈ʒu
    [SerializeField] float spawnDelay = 60f;// �X�|�[���܂ł̎���
    [SerializeField] float moveDuration = 0.3f;// �ړ��ɂ����鎞��

    [SerializeField] GameObject missilePrefab; 
    [SerializeField] GameObject fastMissilePrefab;
    [SerializeField] float spawnInterval = 3f; // �I�u�W�F�N�g�𐶐�����Ԋu�i�b�j
    private bool isSpawning = true; // �����𐧌䂷��t���O
    [SerializeField] float missileDelay = 62f;
    [SerializeField] Transform rightArm;
    [SerializeField] Transform leftArm;
    [SerializeField] Transform teal;

    [SerializeField] int bossHp = 15;
    [SerializeField] int hitDamage = 1;
    [SerializeField] int scoreValue = 150; // ���̓G��|�������̃X�R�A
    private ScoreManager scoreManager;

    void Start()
    {
        transform.position = startPosition;

        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        StartCoroutine(SpawnBoss());
        
        StartCoroutine(Missile());

    }

    void Update()
    {
        pos = transform.position;

        // �i�|�C���g�j�}�C�i�X�������邱�Ƃŋt�����Ɉړ�����B
        transform.Translate(transform.right * Time.deltaTime * 3 * num);

        if (pos.x > 5.5) num = -1;
        if (pos.x < -5.5) num = 1;
        Debug.Log(bossHp);

        if (bossHp == 0)
        {
            Die();
        }
    }

    private IEnumerator SpawnBoss()
    {
        // 1���ҋ@
        yield return new WaitForSeconds(spawnDelay);

        // �ړ�
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // �t���[�����܂���
        }

        // �ŏI�ʒu���m���ɐݒ�
        transform.position = endPosition;
    }

    private IEnumerator Missile()
    {
        yield return new WaitForSeconds(missileDelay);

        while (isSpawning)
        {
            // �I�u�W�F�N�g�𐶐�
            SpawnMissile();

            // ��莞�ԑҋ@
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    void SpawnMissile()
    {
        if (rightArm != null || leftArm != null)
        {
            Transform selectedPosition = null;

            // ���ˈʒu�������_���őI���i���݂�����̂̂݁j
            if (rightArm != null && leftArm != null)
            {
                // �������݂���ꍇ�̓����_���ɑI��
                selectedPosition = Random.Range(0, 2) == 0 ? rightArm : leftArm;
            }
            else if (rightArm != null)
            {
                // �E�A�[���̂ݑ��݂���ꍇ
                selectedPosition = rightArm;
            }
            else if (leftArm != null)
            {
                // ���A�[���̂ݑ��݂���ꍇ
                selectedPosition = leftArm;
            }

            // ���ˈʒu�����肵�Ă���ꍇ�̂݃~�T�C���𐶐�
            if (selectedPosition != null)
            {
                Instantiate(missilePrefab, selectedPosition.position, selectedPosition.rotation);
            }
        }
        else
        {
            // ���A�[�������݂��Ȃ��ꍇ�̃C�x���g
            HandleNoArms();
        }
    }

    void HandleNoArms()
    {
        StartCoroutine(FastMissile());
    }
    private IEnumerator FastMissile()
    {
        while (true) // �K�v�ɉ����ď�����ǉ����ďI���^�C�~���O�𐧌�
        {
            // ����U���Ƃ��ăI�u�W�F�N�g�𐶐�
            SpawnSpecialObject();

            // ��莞�ԑҋ@
            yield return new WaitForSeconds(spawnInterval); // spawnInterval�𗬗p
        }
    }
    void SpawnSpecialObject()
    {
        if (teal != null)
        {
            Transform selectedPosition = null;
            if (selectedPosition != null)
            {
                Instantiate(fastMissilePrefab, selectedPosition.position, selectedPosition.rotation);
            }
        }
    }
    void OnMouseDown()
    {
        bossHp -= hitDamage;
    }

    public void Die()
    {
        // �X�R�A�����Z
        scoreManager.AddScore(scoreValue);

        // �G���폜
        Destroy(gameObject);
    }
}
