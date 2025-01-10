using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UIのためのライブラ��E

public class PlayerController : MonoBehaviour
{
    // マウス了崔と��E`��E瀕志辰魃４罎垢�E燭瓩��篳�
    Vector3 mousePos, worldPos;

    [SerializeField] int maxPlayerHp = 100; // 悍偕HP
    private int playerHp; // �F壓のHP
    public Slider healthSlider; // UIのHPバ�`

    [SerializeField] float maxPlayerMp = 100f; // 悍偕MP
    [SerializeField] float mpDecreaseRate = 5f; // MPが�p��E拔�
    private float currentPlayerMp; // �F壓のMP
    public Slider mpSlider; // UIのMPバ�`

    public PopUpController popUpController; // ポップアップを凹羮す��E灰鵐肇��`ラ�`
    [SerializeField] float mpIncreaseInterval = 1f; // MPが指甠垢�Er�g�g��E
    [SerializeField] int mpIncreaseAmount = 1; // 1指のMP指畽�
    [SerializeField] float popUpChance = 0.1f; // ポップアップが凹羮さ��E�E_楕��0.0 - 1.0��

    private bool isPopUpWaiting = false; // ポップアップが棋�C嶄かどうかのフラグ

    // ゲ�`ム�_兵�rに1指だけ�g佩さ��E�E
    void Start()
    {
        playerHp = maxPlayerHp; // プ��Eぅ筴`のHPを悍偕�､穆O協
        currentPlayerMp = maxPlayerMp; // プ��Eぅ筴`のMPを悍偕�､穆O協

        // HPバ�`の�O協
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxPlayerHp; // HPバ�`の悍偕�､鰓O協
            healthSlider.value = playerHp; // �F壓のHPをバ�`に�O協
        }

        // MPバ�`の�O協
        if (mpSlider != null)
        {
            mpSlider.maxValue = maxPlayerMp; // MPバ�`の悍偕�､鰓O協
            mpSlider.value = currentPlayerMp; // �F壓のMPをバ�`に�O協
        }

        // 匯協�r�gごとにMPを指甠垢�EI煽栓�_兵
        StartCoroutine(IncreaseMpOverTime());
    }

    // �哀侫�E`ム�g佩さ��E�E
    void Update()
    {
        // マウスの了崔を函誼
        mousePos = Input.mousePosition;

        // マウスのスク��E`ン恙�砲鬟�E`��E夫��砲����Q
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        // プ��Eぅ筴`の了崔をマウスの了崔に栽��E擦�E
        transform.position = worldPos;

        // マウスの恣ク��E奪�でMPを�pらす
        if (Input.GetMouseButtonDown(0))
        {
            ReduceMp(); // MPを�pらす
        }

        //
        if (playerHp <= 0)
        {
            // Call Game Over when health is zero
            FindObjectOfType<GameManager>().GameOver();
        }
    }

    // ダメ�`ジを鞭けたときに柵ば��E�E
    public void AddDamage(int damage)
    {
        maxPlayerHp -= damage; // HPを�pらす

        // HPが0參和にならないように崙��
        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);

        // HPバ�`を��E�
        if (healthSlider != null)
        {
            healthSlider.value = playerHp;
        }
    }

    // MPを�pらす�I��E
    void ReduceMp()
    {
        // MPが0參和にならないように崙��
        currentPlayerMp = Mathf.Clamp(currentPlayerMp - mpDecreaseRate, 0, maxPlayerMp);

        // MPバ�`を��E�
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // MPを指甠垢�EI��E
    void IncreaseMp()
    {
        // MPが悍偕�､魍�えないように崙��
        currentPlayerMp = Mathf.Min(currentPlayerMp + mpIncreaseAmount, maxPlayerMp);

        // MPバ�`を��E�
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // 匯協�r�gごとにMPを指甠垢�E灰�E`チ��E
    IEnumerator IncreaseMpOverTime()
    {
        Debug.Log("MP Recovery Coroutine Started!");
        while (true) // �o�泪�E`プでMPを指��
        {
            // MPを指��
            IncreaseMp();
            yield return new WaitForSeconds(mpIncreaseInterval);

            // MPが悍偕ではなく、ポップアップの�_楕に児づいてポップアップを凹羮
            if (currentPlayerMp < maxPlayerMp && Random.value <= popUpChance)
            {
                // ポップアップを凹羮
                if (popUpController != null)
                {
                    popUpController.StartRandomPopUpCoroutine(true); // ポップアップを凹羮
                    isPopUpWaiting = true;
                }
            }

            // 肝の指甠泙粘��C
            yield return new WaitForSeconds(mpIncreaseInterval);

            // ポップアップ棋�C嶄のフラグが羨ってい��E弌�20��E瓩縫�E札奪�
            if (isPopUpWaiting)
            {
                yield return new WaitForSeconds(20f); // 20��E�つ
                isPopUpWaiting = false; // フラグを��E札奪箸靴董�肝のポップアップを�S辛
            }
        }
    }
}