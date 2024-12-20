using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UIのためのライブラリ

public class PlayerController : MonoBehaviour
{
    // マウス了崔とワ�`ルド了崔を隠贋するための�篳�
    Vector3 mousePos, worldPos;

    [SerializeField] int maxPlayerHp = 100; // 恷寄HP
    private int playerHp; // �F壓のHP
    public Slider healthSlider; // UIのHPバ�`

    [SerializeField] float maxPlayerMp = 100f; // 恷寄MP
    [SerializeField] float mpDecreaseRate = 5f; // MPが�pる堀業
    private float currentPlayerMp; // �F壓のMP
    public Slider mpSlider; // UIのMPバ�`

    public PopUpController popUpController; // ポップアップを燕幣するコントロ�`ラ�`
    [SerializeField] float mpIncreaseInterval = 1f; // MPが指甠垢��r�g�g侯
    [SerializeField] int mpIncreaseAmount = 1; // 1指のMP指畽�
    [SerializeField] float popUpChance = 0.1f; // ポップアップが燕幣される�_楕��0.0 - 1.0��

    private bool isPopUpWaiting = false; // ポップアップが棋�C嶄かどうかのフラグ

    // ゲ�`ム�_兵�rに1指だけ�g佩される
    void Start()
    {
        playerHp = maxPlayerHp; // プレイヤ�`のHPを恷寄�､穆O協
        currentPlayerMp = maxPlayerMp; // プレイヤ�`のMPを恷寄�､穆O協

        // HPバ�`の�O協
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxPlayerHp; // HPバ�`の恷寄�､鰓O協
            healthSlider.value = playerHp; // �F壓のHPをバ�`に�O協
        }

        // MPバ�`の�O協
        if (mpSlider != null)
        {
            mpSlider.maxValue = maxPlayerMp; // MPバ�`の恷寄�､鰓O協
            mpSlider.value = currentPlayerMp; // �F壓のMPをバ�`に�O協
        }

        // 匯協�r�gごとにMPを指甠垢��I尖を�_兵
        StartCoroutine(IncreaseMpOverTime());
    }

    // �哀侫讒`ム�g佩される
    void Update()
    {
        // マウスの了崔を函誼
        mousePos = Input.mousePosition;

        // マウスのスクリ�`ン恙�砲鬟鍠`ルド恙�砲����Q
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        // プレイヤ�`の了崔をマウスの了崔に栽わせる
        transform.position = worldPos;

        // マウスの恣クリックでMPを�pらす
        if (Input.GetMouseButtonDown(0))
        {
            ReduceMp(); // MPを�pらす
        }
    }

    // ダメ�`ジを鞭けたときに柵ばれる
    public void AddDamage(int damage)
    {
        maxPlayerHp -= damage; // HPを�pらす

        // HPが0參和にならないように崙��
        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);

        // HPバ�`を厚仟
        if (healthSlider != null)
        {
            healthSlider.value = playerHp;
        }
    }

    // MPを�pらす�I尖
    void ReduceMp()
    {
        // MPが0參和にならないように崙��
        currentPlayerMp = Mathf.Clamp(currentPlayerMp - mpDecreaseRate, 0, maxPlayerMp);

        // MPバ�`を厚仟
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // MPを指甠垢��I尖
    void IncreaseMp()
    {
        // MPが恷寄�､魍�えないように崙��
        currentPlayerMp = Mathf.Min(currentPlayerMp + mpIncreaseAmount, maxPlayerMp);

        // MPバ�`を厚仟
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // 匯協�r�gごとにMPを指甠垢襯灰覃`チン
    IEnumerator IncreaseMpOverTime()
    {
        Debug.Log("MP Recovery Coroutine Started!");
        while (true) // �o�泪覃`プでMPを指��
        {
            // MPを指��
            IncreaseMp();
            yield return new WaitForSeconds(mpIncreaseInterval);

            // MPが恷寄ではなく、ポップアップの�_楕に児づいてポップアップを燕幣
            if (currentPlayerMp < maxPlayerMp && Random.value <= popUpChance)
            {
                // ポップアップを燕幣
                if (popUpController != null)
                {
                    popUpController.StartRandomPopUpCoroutine(true); // ポップアップを燕幣
                    isPopUpWaiting = true;
                }
            }

            // 肝の指甠泙粘��C
            yield return new WaitForSeconds(mpIncreaseInterval);

            // ポップアップ棋�C嶄のフラグが羨っていれば、20昼瘁にリセット
            if (isPopUpWaiting)
            {
                yield return new WaitForSeconds(20f); // 20昼棋つ
                isPopUpWaiting = false; // フラグをリセットして、肝のポップアップを�S辛
            }
        }
    }
}