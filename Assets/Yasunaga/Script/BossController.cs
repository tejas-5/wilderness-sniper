using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController: MonoBehaviour
{
    private Vector2 pos;
    [SerializeField] int num = 1;//����

    [SerializeField] GameObject bossObject; 
    [SerializeField] Vector3 startPosition; // �����ʒu
    [SerializeField] Vector3 endPosition;    // �ړ���̈ʒu
    [SerializeField] float spawnDelay = 60f;// �X�|�[���܂ł̎���
    [SerializeField] float moveDuration = 0.3f;// �ړ��ɂ����鎞��

    [SerializeField] GameObject missilePrefab; 
    [SerializeField] float spawnInterval = 3f; // �I�u�W�F�N�g�𐶐�����Ԋu�i�b�j
    private bool isSpawning = true; // �����𐧌䂷��t���O
    [SerializeField] float missileDelay = 62f;
    [SerializeField] Transform rightArm;
    [SerializeField] Transform leftArm;

    void Start()
    {
        transform.position = startPosition;

        if (bossObject != null) StartCoroutine(SpawnBoss());
        
        if (bossObject != null) StartCoroutine(Missile());
    }

    void Update()
    {
        pos = transform.position;

        // �i�|�C���g�j�}�C�i�X�������邱�Ƃŋt�����Ɉړ�����B
        transform.Translate(transform.right * Time.deltaTime * 3 * num);

        if (pos.x > 5.5) num = -1;
        if (pos.x < -5.5) num = 1;
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
        if (missilePrefab != null &&
            rightArm != null�@&&
            leftArm != null)
        {
            // ���ˈʒu�������_���őI��
            Transform selectedPosition = Random.Range(0, 2) == 0 ? rightArm : leftArm;

            // �w�肳�ꂽ�ʒu�Ɖ�]�ŃI�u�W�F�N�g�𐶐�
            Instantiate(missilePrefab, selectedPosition.position, selectedPosition.rotation);
        }
    }
}
