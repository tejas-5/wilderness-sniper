using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    //�{�X�ړ�
    private Vector2 pos;
    private int num = 1;//����
    [SerializeField] int moveSpeed = 3;

    //�{�X�X�|�[��
    [SerializeField] float spawnDelay = 60f;// �X�|�[���܂ł̎���
    [SerializeField] Vector3 startPosition; // �����ʒu
    [SerializeField] Vector3 endPosition;    // �ŏI�ʒu
    [SerializeField] float moveDuration = 0.3f; //�ŏI�ʒu�ړ��܂ł����鎞��

    //�n�T�~����̍U��
    private bool isArmAttack = true; // �����𐧌䂷��t���O
    [SerializeField] float missileDelay = 62f;//�~�T�C�����˂܂ł̎���
    [SerializeField] GameObject missilePrefab;
    [SerializeField] float armInterval = 3f; // �I�u�W�F�N�g�𐶐�����Ԋu�i�b�j
    [SerializeField] Transform rightArm;
    [SerializeField] Transform leftArm;
    //�K������̍U��
    private bool isTealAttack = false;
    [SerializeField] GameObject fastMissilePrefab;
    [SerializeField] float tealInterval = 2f;
    [SerializeField] Transform teal;

    //�p�����[�^�[
    [SerializeField] int bossHp = 20;
    [SerializeField] int hitDamage = 1;
    [SerializeField] int scoreValue = 150; // ���̓G��|�������̃X�R�A
    private ScoreManager scoreManager;

    void Start()
    {
        transform.position = startPosition;

        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        StartCoroutine(SpawnBoss());

        StartCoroutine(ArmAttack());
    }

    void Update()
    {
        pos = transform.position;

        // �}�C�i�X�������邱�Ƃŋt�����Ɉړ�
        transform.Translate(transform.right * Time.deltaTime * moveSpeed * num);
        if (pos.x > 5.5) num = -1;
        if (pos.x < -5.5) num = 1;

        if (bossHp == 0) Die();
    }

    //�{�X�X�|�[��
    private IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(spawnDelay);

        // �ŏI�ʒu�܂ňړ�
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;

            yield return null;

            yield return null; 

        }
        //�ŏI�ʒu��ݒ�
        transform.position = endPosition;
    }

    //�n�T�~�U��
    private IEnumerator ArmAttack()
    {
        yield return new WaitForSeconds(missileDelay);

        while (isArmAttack)
        {
            ArmMissile();
            yield return new WaitForSeconds(armInterval);
        }
    }
    //�n�T�~�~�T�C��
    void ArmMissile()
    {
        if (rightArm != null || leftArm != null)
        {
            Transform selectedPosition = null;

            // ���ˈʒu�������_���őI��
            if (rightArm != null && leftArm != null) 
                selectedPosition = Random.Range(0, 2) == 0 ? rightArm : leftArm;
            // �E�n�T�~�̂ݑ��݂���ꍇ
            else if (rightArm != null) 
                selectedPosition = rightArm;
            // ���n�T�~�̂ݑ��݂���ꍇ
            else if (leftArm != null) 
                selectedPosition = leftArm;

            // ���ˈʒu�����肵�Ă���ꍇ�̂݃~�T�C���𐶐�
            if (selectedPosition != null) 
                Instantiate(missilePrefab, selectedPosition.position, selectedPosition.rotation);
        }
        //���n�T�~���j�󂳂ꂽ��K���U��
        else if (!isTealAttack)
        {
            isTealAttack = true; // �K���U���J�n�t���O��ݒ�
            StartCoroutine(TealAttack());
        }
    }

    //�K���U��
    private IEnumerator TealAttack()
    {

        while (isTealAttack)
        {
            TealMissile();
            yield return new WaitForSeconds(tealInterval);

            while (isTealAttack)
            {
                TealMissile();
                yield return new WaitForSeconds(tealInterval);

            }
        }
        //�K���~�T�C��
        void TealMissile()
        {
            if (teal != null)
                Instantiate(fastMissilePrefab, teal.position, teal.rotation);
        }   
    }
    void Die()
    {
        // �X�R�A�����Z
        scoreManager.AddScore(scoreValue);

        Destroy(gameObject);
    }
}