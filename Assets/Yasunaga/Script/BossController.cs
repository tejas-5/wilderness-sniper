using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController: MonoBehaviour
{
    private Vector2 pos;
    [SerializeField] int num = 1;

    [SerializeField] GameObject bossObject; // �L��������Ώۂ̃I�u�W�F�N�g
    [SerializeField] Vector3 startPosition; // �����ʒu
    [SerializeField] Vector3 endPosition;    // �ړ���̈ʒu
    [SerializeField] float spawnDelay;// �X�|�[���܂ł̎���
    [SerializeField] float moveDuration;// �ړ��ɂ����鎞��

    [SerializeField] GameObject missilePrefab; // ��������I�u�W�F�N�g�̃v���n�u
    [SerializeField] Transform spawnPoint;  // �����ʒu�i�{�X�̑O����͂Ȃǁj
    [SerializeField] float spawnInterval; // �I�u�W�F�N�g�𐶐�����Ԋu�i�b�j
    private bool isSpawning = true; // �����𐧌䂷��t���O
    [SerializeField] float missileDelay;

    void Start()
    {
        transform.position = startPosition;

        if (bossObject != null) StartCoroutine(SpawnBoss());
        
        if (bossObject != null) StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        pos = transform.position;

        // �i�|�C���g�j�}�C�i�X�������邱�Ƃŋt�����Ɉړ�����B
        transform.Translate(transform.right * Time.deltaTime * 3 * num);

        if (pos.x > 5.5)
        {
            num = -1;
        }
        if (pos.x < -5.5)
        {
            num = 1;
        }
    }

    private IEnumerator SpawnBoss()
    {
        // 1���ҋ@
        yield return new WaitForSeconds(spawnDelay);

        bossObject.SetActive(true);

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

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(missileDelay);

        while (isSpawning)
        {
            // �I�u�W�F�N�g�𐶐�
            SpawnObject();

            // ��莞�ԑҋ@
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    void SpawnObject()
    {
        if (missilePrefab != null && spawnPoint != null)
        {
            // �w�肳�ꂽ�ʒu�ŃI�u�W�F�N�g�𐶐�
            Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
