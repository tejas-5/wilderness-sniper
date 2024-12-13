using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI���g���ꍇ


public class PlayerController : MonoBehaviour
{
    //���W�p�̕ϐ�
    Vector3 mousePos, worldPos;

    [SerializeField] int maxPlayerHp = 200;
    private int playerHp;
    public Slider healthSlider; // UI�̃X���C�_�[�ő̗͂�\��

    [SerializeField] int maxPlayerMp = 100;
    [SerializeField] int downMp = 10;
    public Slider mpSlider;

    void Start()
    {
        playerHp = maxPlayerHp;

        // UI�̃X���C�_�[���������i�I�v�V�����j
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxPlayerHp;
            healthSlider.value = playerHp;
        }
        if (mpSlider != null)
        {
            mpSlider.maxValue = maxPlayerMp;
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

        if (mpSlider != null)
        {
            mpSlider.value = maxPlayerMp;
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
        // MP������
        maxPlayerMp = Mathf.Max(0, maxPlayerMp - downMp);
    }


}

