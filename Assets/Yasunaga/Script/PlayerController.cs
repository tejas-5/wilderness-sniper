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
    }

    void Update()
    {
        //�}�E�X���W�̎擾
        mousePos = Input.mousePosition;
        //�X�N���[�����W�����[���h���W�ɕϊ�
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        //���[���h���W�����g�̍��W�ɐݒ�
        transform.position = worldPos;
        Debug.Log(maxPlayerHp);

        if (Input.GetMouseButtonDown(0)) // 0�͍��N���b�N
        {
            ReduceMp();
        }

        // ���Ԍo�߂�MP������
        ReduceMpOverTime();
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
        currentPlayerMp = Mathf.Clamp(currentPlayerMp + upMp, 0, maxPlayerMp);

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


}

