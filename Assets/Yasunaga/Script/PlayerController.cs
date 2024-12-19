using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI���g�����߂ɕK�v

public class PlayerController : MonoBehaviour
{
    // �v���C���[�̍��W���}�E�X�̈ʒu�Ō��߂邽�߂̕ϐ�
    Vector3 mousePos, worldPos;

    [SerializeField] int maxPlayerHp = 100; // �ő�HP
    private int playerHp; // ���݂�HP
    public Slider healthSlider; // UI�̃X���C�_�[��HP��\��

    [SerializeField] float maxPlayerMp = 100f; // �ő�MP
    [SerializeField] float mpDecreaseRate = 5f; // MP�����b�����
    private float currentPlayerMp; // ���݂�MP
    public Slider mpSlider; // UI�̃X���C�_�[��MP��\��

    public PopUpController popUpController; // �|�b�v�A�b�v�\���𐧌䂷��N���X
    [SerializeField] float mpIncreaseInterval = 1f; // MP����������Ԋu�i�b�j
    [SerializeField] int mpIncreaseAmount = 1; // 1���MP������
    [SerializeField] float popUpChance = 0.1f; // �|�b�v�A�b�v���\�������m���i0.0 - 1.0�j

    private bool isPopUpWaiting = false;

    // �Q�[���J�n���ɌĂ΂��
    void Start()
    {
        playerHp = maxPlayerHp; // �v���C���[��HP���ő�ɐݒ�
        currentPlayerMp = maxPlayerMp; // �v���C���[��MP���ő�ɐݒ�

        // HP�X���C�_�[���������iUI�ɕ\���j
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxPlayerHp; // HP�X���C�_�[�̍ő�l��ݒ�
            healthSlider.value = playerHp; // ���݂�HP�X���C�_�[�̒l��ݒ�
        }

        // MP�X���C�_�[���������iUI�ɕ\���j
        if (mpSlider != null)
        {
            mpSlider.maxValue = maxPlayerMp; // MP�X���C�_�[�̍ő�l��ݒ�
            mpSlider.value = currentPlayerMp; // ���݂�MP�X���C�_�[�̒l��ݒ�
        }

        // MP�����Ԍo�߂ő���������R���[�`�����J�n
        StartCoroutine(IncreaseMpOverTime());
    }

    // ���t���[���Ă΂��
    void Update()
    {
        // �}�E�X�̍��W���擾
        mousePos = Input.mousePosition;

        // �X�N���[�����W�����[���h���W�ɕϊ�
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        // �v���C���[�̈ʒu���}�E�X�̈ʒu�ɐݒ�
        transform.position = worldPos;

        // �������N���b�N�������ꂽ��
        if (Input.GetMouseButtonDown(0))
        {
            ReduceMp(); // MP�����炷
        }
    }

    // �v���C���[�Ƀ_���[�W��^����֐�
    public void AddDamage(int damage)
    {
        maxPlayerHp -= damage; // �ő�HP���_���[�W�����炷

        // HP��0�����ɂȂ�Ȃ��悤�ɐ���
        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);

        // HP�X���C�_�[���X�V
        if (healthSlider != null)
        {
            healthSlider.value = playerHp;
        }
    }

    // MP�����炷�֐�
    void ReduceMp()
    {
        // MP�����������A0�����ɂȂ�Ȃ��悤�ɐ���
        currentPlayerMp = Mathf.Clamp(currentPlayerMp - mpDecreaseRate, 0, maxPlayerMp);

        // MP�X���C�_�[���X�V
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // MP�𑝉�������֐�
    void IncreaseMp()
    {
        // MP�𑝉������A�ő�l�𒴂��Ȃ��悤�ɐ���
        currentPlayerMp = Mathf.Min(currentPlayerMp + mpIncreaseAmount, maxPlayerMp);

        // MP�X���C�_�[���X�V
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // MP�����Ԍo�߂ő���������R���[�`��
    IEnumerator IncreaseMpOverTime()
    {
        while (true) // �������[�v��MP�𑝉�������
        {
            //if (!isPopUpWaiting)
            //{
                // MP�𑝉�������
                IncreaseMp();

                // ����MP���ő�l�����ŁA�����_���Ȋm���Ń|�b�v�A�b�v��\��
                if (currentPlayerMp < maxPlayerMp && Random.value <= popUpChance)
                {
                    // �|�b�v�A�b�v���\�������`�����X��������
                    if (popUpController != null)
                    {
                        popUpController.StartRandomPopUpCoroutine(true); // �|�b�v�A�b�v���J�n
                        isPopUpWaiting = true;
                    }
                }
            //}
            // �w�肵�����Ԃ����҂�
            yield return new WaitForSeconds(mpIncreaseInterval);
            if (isPopUpWaiting)
            {
                yield return new WaitForSeconds(20f); // Wait for 20 seconds
                isPopUpWaiting = false; // Reset the flag to allow another pop-up
            }
            
        }
    }

}
