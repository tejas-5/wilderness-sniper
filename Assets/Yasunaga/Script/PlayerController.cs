using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI�Τ���Υ饤�֥饁E

public class PlayerController : MonoBehaviour
{
    // �ޥ���λ�äȥ�E`��E�λ�ä򱣴椹��E���Ή���
    Vector3 mousePos, worldPos;

    [SerializeField] int maxPlayerHp = 100; // ����HP
    private int playerHp; // �F�ڤ�HP
    public Slider healthSlider; // UI��HP�Щ`

    [SerializeField] float maxPlayerMp = 100f; // ����MP
    [SerializeField] float mpDecreaseRate = 5f; // MP���p��Eٶ�
    private float currentPlayerMp; // �F�ڤ�MP
    public Slider mpSlider; // UI��MP�Щ`

    public PopUpController popUpController; // �ݥåץ��åפ��澤���E���ȥ��`��`
    [SerializeField] float mpIncreaseInterval = 1f; // MP���؏ͤ���Er�g�g��E
    [SerializeField] int mpIncreaseAmount = 1; // 1�ؤ�MP�؏���
    [SerializeField] float popUpChance = 0.1f; // �ݥåץ��åפ���澤���E�E_�ʣ�0.0 - 1.0��

    private bool isPopUpWaiting = false; // �ݥåץ��åפ����C�Ф��ɤ����Υե饰

    // ���`���_ʼ�r��1�ؤ����g�Ф���E�E
    void Start()
    {
        playerHp = maxPlayerHp; // �ץ�E���`��HP�����󂎤��O��
        currentPlayerMp = maxPlayerMp; // �ץ�E���`��MP�����󂎤��O��

        // HP�Щ`���O��
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxPlayerHp; // HP�Щ`�����󂎤��O��
            healthSlider.value = playerHp; // �F�ڤ�HP��Щ`���O��
        }

        // MP�Щ`���O��
        if (mpSlider != null)
        {
            mpSlider.maxValue = maxPlayerMp; // MP�Щ`�����󂎤��O��
            mpSlider.value = currentPlayerMp; // �F�ڤ�MP��Щ`���O��
        }

        // һ���r�g���Ȥ�MP��؏ͤ���EI�����_ʼ
        StartCoroutine(IncreaseMpOverTime());
    }

    // ���ե�E`���g�Ф���E�E
    void Update()
    {
        // �ޥ�����λ�ä�ȡ��
        mousePos = Input.mousePosition;

        // �ޥ����Υ�����E`�����ˤ�E`��E����ˤˉ�Q
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        // �ץ�E���`��λ�ä�ޥ�����λ�ä˺Ϥ�E���E
        transform.position = worldPos;

        // �ޥ������󥯥�Eå���MP��p�餹
        if (Input.GetMouseButtonDown(0))
        {
            ReduceMp(); // MP��p�餹
        }
    }

    // ����`�����ܤ����Ȥ��˺��Ф�E�E
    public void AddDamage(int damage)
    {
        maxPlayerHp -= damage; // HP��p�餹

        // HP��0���¤ˤʤ�ʤ��褦������
        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);

        // HP�Щ`��E�
        if (healthSlider != null)
        {
            healthSlider.value = playerHp;
        }
    }

    // MP��p�餹�I��E
    void ReduceMp()
    {
        // MP��0���¤ˤʤ�ʤ��褦������
        currentPlayerMp = Mathf.Clamp(currentPlayerMp - mpDecreaseRate, 0, maxPlayerMp);

        // MP�Щ`��E�
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // MP��؏ͤ���EI��E
    void IncreaseMp()
    {
        // MP�����󂎤򳬤��ʤ��褦������
        currentPlayerMp = Mathf.Min(currentPlayerMp + mpIncreaseAmount, maxPlayerMp);

        // MP�Щ`��E�
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // һ���r�g���Ȥ�MP��؏ͤ���E���E`����E
    IEnumerator IncreaseMpOverTime()
    {
        Debug.Log("MP Recovery Coroutine Started!");
        while (true) // �o�ޥ�E`�פ�MP��؏�
        {
            // MP��؏�
            IncreaseMp();
            yield return new WaitForSeconds(mpIncreaseInterval);

            // MP������ǤϤʤ����ݥåץ��åפδ_�ʤ˻��Ť��ƥݥåץ��åפ���
            if (currentPlayerMp < maxPlayerMp && Random.value <= popUpChance)
            {
                // �ݥåץ��åפ���
                if (popUpController != null)
                {
                    popUpController.StartRandomPopUpCoroutine(true); // �ݥåץ��åפ���
                    isPopUpWaiting = true;
                }
            }

            // �Τλ؏ͤޤǴ��C
            yield return new WaitForSeconds(mpIncreaseInterval);

            // �ݥåץ��å״��C�ФΥե饰�����äƤ���EС�20ÁE�˥�E��å�
            if (isPopUpWaiting)
            {
                yield return new WaitForSeconds(20f); // 20ÁE���
                isPopUpWaiting = false; // �ե饰��E��åȤ��ơ��ΤΥݥåץ��åפ��S��
            }
        }
    }
}