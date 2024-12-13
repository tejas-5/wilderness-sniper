using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI���g���ꍇ


public class PlayerController : MonoBehaviour
{
    //���W�p�̕ϐ�
    Vector3 mousePos, worldPos;

    [SerializeField] int maxPlayerHp = 100;
    private int playerHp;
    public Slider healthSlider; // UI�̃X���C�_�[�ő̗͂�\��

    [SerializeField] float maxPlayerMp = 100f;
    [SerializeField] float upMp = 10f; // MP������
    [SerializeField] float mpDecreaseRate = 5f; // ���b��������MP��
    private float currentPlayerMp; // ���݂�MP
    public Slider mpSlider;

    public PopUpController popUpController;
    [SerializeField] float mpIncreaseInterval = 1f; // MP����������Ԋu�i�b�j
    [SerializeField] int mpIncreaseAmount = 1; // 1��̑�����
    [SerializeField] float popUpChance = 0.5f; // �|�b�v�A�b�v���\�������m���i0.0 - 1.0�j

    void Start()
    {
        playerHp = maxPlayerHp;
        currentPlayerMp = maxPlayerMp; // MP�͍ő�l�ŊJ�n

        // UI�̃X���C�_�[���������i�I�v�V�����j
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxPlayerHp;
            healthSlider.value = playerHp;
        }
        if (mpSlider != null)
        {
            mpSlider.maxValue = maxPlayerMp;
            mpSlider.value = currentPlayerMp;
        }
        // MP�����Ԍo�߂ő�������R���[�`�����J�n
        StartCoroutine(IncreaseMpOverTime());
        
    }

    void Update()
    {
        //�}�E�X���W�̎擾
        mousePos = Input.mousePosition;
        //�X�N���[�����W�����[���h���W�ɕϊ�
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        //���[���h���W�����g�̍��W�ɐݒ�
        transform.position = worldPos;
        //Debug.Log(maxPlayerHp);
        //Debug.Log(maxPlayerMp);

        if (Input.GetMouseButtonDown(0)) // 0�͍��N���b�N
        {
            ReduceMp();
        }
    }

    public void AddDamage(int damage)
    {
        maxPlayerHp -= damage;

        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);

        // UI�̃X���C�_�[���X�V
        if (healthSlider != null)
        {
            healthSlider.value = playerHp;
        }
    }

    void ReduceMp()
    {
        // MP�𑝉�
        currentPlayerMp = Mathf.Clamp(currentPlayerMp - mpDecreaseRate, 0, maxPlayerMp);

        // �X���C�_�[�̒l���X�V
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    void ReduceMpOverTime()
    {
        // ���Ԍo�߂Ɋ�Â���MP������
        currentPlayerMp = Mathf.Clamp(currentPlayerMp - mpDecreaseRate * Time.deltaTime, 0, maxPlayerMp);

        // �X���C�_�[�̒l���X�V
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    void IncreaseMp()
    {
        // Increase the current MP by the specified amount
        currentPlayerMp = Mathf.Min(currentPlayerMp + mpIncreaseAmount, maxPlayerMp); // Ensure current MP doesn't exceed max MP
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp; // Update the UI slider
        }
    }
    // �R���[�`����MP�����Ԍo�߂ő���������
    IEnumerator IncreaseMpOverTime()
    {
        while (true) // �������[�v��MP�𑝉���������
        {
            // MP��������
            IncreaseMp();

            // �`�����X�̊m���Ń|�b�v�A�b�v�\��
            if (currentPlayerMp < maxPlayerMp && Random.value <= popUpChance) // �`�����X�̊m���Ń|�b�v�A�b�v�\��
            {
                if (popUpController != null)
                {
                    popUpController.StartRandomPopUpCoroutine(true); // Start the pop-up coroutine here
                }
            }

            // �w�肵���Ԋu�����ҋ@
            yield return new WaitForSeconds(mpIncreaseInterval);
        }
    }
    void ShowPopUp()
    {
        if (popUpController != null)
        {
            // �|�b�v�A�b�v��\�����邽�߂̊֐����Ăяo��
            popUpController.ShowPopUp();
        }
    }
}