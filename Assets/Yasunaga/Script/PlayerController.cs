using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI�Τ���Υ饤�֥��

public class PlayerController : MonoBehaviour
{
    // �ޥ���λ�äȥ�`���λ�ä򱣴椹�뤿��Ή���
    Vector3 mousePos, worldPos;

    [SerializeField] int maxPlayerHp = 100; // ���HP
    private int playerHp; // �F�ڤ�HP
    public Slider healthSlider; // UI��HP�Щ`

    [SerializeField] float maxPlayerMp = 100f; // ���MP
    [SerializeField] float mpDecreaseRate = 5f; // MP���p���ٶ�
    private float currentPlayerMp; // �F�ڤ�MP
    public Slider mpSlider; // UI��MP�Щ`

    public PopUpController popUpController; // �ݥåץ��åפ��ʾ���륳��ȥ�`��`
    [SerializeField] float mpIncreaseInterval = 1f; // MP���؏ͤ���r�g�g��
    [SerializeField] int mpIncreaseAmount = 1; // 1�ؤ�MP�؏���
    [SerializeField] float popUpChance = 0.1f; // �ݥåץ��åפ���ʾ�����_�ʣ�0.0 - 1.0��

    private bool isPopUpWaiting = false; // �ݥåץ��åפ����C�Ф��ɤ����Υե饰

    // ���`���_ʼ�r��1�ؤ����g�Ф����
    void Start()
    {
        playerHp = maxPlayerHp; // �ץ쥤��`��HP����󂎤��O��
        currentPlayerMp = maxPlayerMp; // �ץ쥤��`��MP����󂎤��O��

        // HP�Щ`���O��
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxPlayerHp; // HP�Щ`����󂎤��O��
            healthSlider.value = playerHp; // �F�ڤ�HP��Щ`���O��
        }

        // MP�Щ`���O��
        if (mpSlider != null)
        {
            mpSlider.maxValue = maxPlayerMp; // MP�Щ`����󂎤��O��
            mpSlider.value = currentPlayerMp; // �F�ڤ�MP��Щ`���O��
        }

        // һ���r�g���Ȥ�MP��؏ͤ���I����_ʼ
        StartCoroutine(IncreaseMpOverTime());
    }

    // ���ե�`���g�Ф����
    void Update()
    {
        // �ޥ�����λ�ä�ȡ��
        mousePos = Input.mousePosition;

        // �ޥ����Υ�����`�����ˤ��`������ˤˉ�Q
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        // �ץ쥤��`��λ�ä�ޥ�����λ�ä˺Ϥ碌��
        transform.position = worldPos;

        // �ޥ������󥯥�å���MP��p�餹
        if (Input.GetMouseButtonDown(0))
        {
            ReduceMp(); // MP��p�餹
        }
    }

    // ����`�����ܤ����Ȥ��˺��Ф��
    public void AddDamage(int damage)
    {
        maxPlayerHp -= damage; // HP��p�餹

        // HP��0���¤ˤʤ�ʤ��褦������
        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);

        // HP�Щ`�����
        if (healthSlider != null)
        {
            healthSlider.value = playerHp;
        }
    }

    // MP��p�餹�I��
    void ReduceMp()
    {
        // MP��0���¤ˤʤ�ʤ��褦������
        currentPlayerMp = Mathf.Clamp(currentPlayerMp - mpDecreaseRate, 0, maxPlayerMp);

        // MP�Щ`�����
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // MP��؏ͤ���I��
    void IncreaseMp()
    {
        // MP����󂎤򳬤��ʤ��褦������
        currentPlayerMp = Mathf.Min(currentPlayerMp + mpIncreaseAmount, maxPlayerMp);

        // MP�Щ`�����
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // һ���r�g���Ȥ�MP��؏ͤ��륳��`����
    IEnumerator IncreaseMpOverTime()
    {
        Debug.Log("MP Recovery Coroutine Started!");
        while (true) // �o�ޥ�`�פ�MP��؏�
        {
            // MP��؏�
            IncreaseMp();
            yield return new WaitForSeconds(mpIncreaseInterval);

            // MP�����ǤϤʤ����ݥåץ��åפδ_�ʤ˻��Ť��ƥݥåץ��åפ��ʾ
            if (currentPlayerMp < maxPlayerMp && Random.value <= popUpChance)
            {
                // �ݥåץ��åפ��ʾ
                if (popUpController != null)
                {
                    popUpController.StartRandomPopUpCoroutine(true); // �ݥåץ��åפ��ʾ
                    isPopUpWaiting = true;
                }
            }

            // �Τλ؏ͤޤǴ��C
            yield return new WaitForSeconds(mpIncreaseInterval);

            // �ݥåץ��å״��C�ФΥե饰�����äƤ���С�20����˥ꥻ�å�
            if (isPopUpWaiting)
            {
                yield return new WaitForSeconds(20f); // 20�����
                isPopUpWaiting = false; // �ե饰��ꥻ�åȤ��ơ��ΤΥݥåץ��åפ��S��
            }
        }
    }
}