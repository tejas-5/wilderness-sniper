using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UIを使うために必要

public class PlayerController : MonoBehaviour
{
    // プレイヤーの座標をマウスの位置で決めるための変数
    Vector3 mousePos, worldPos;

    [SerializeField] int maxPlayerHp = 100; // 最大HP
    private int playerHp; // 現在のHP
    public Slider healthSlider; // UIのスライダーでHPを表示

    [SerializeField] float maxPlayerMp = 100f; // 最大MP
    [SerializeField] float mpDecreaseRate = 5f; // MPが毎秒減る量
    private float currentPlayerMp; // 現在のMP
    public Slider mpSlider; // UIのスライダーでMPを表示

    public PopUpController popUpController; // ポップアップ表示を制御するクラス
    [SerializeField] float mpIncreaseInterval = 1f; // MPが増加する間隔（秒）
    [SerializeField] int mpIncreaseAmount = 1; // 1回のMP増加量
    [SerializeField] float popUpChance = 0.1f; // ポップアップが表示される確率（0.0 - 1.0）

    private bool isPopUpWaiting = false;

    // ゲーム開始時に呼ばれる
    void Start()
    {
        playerHp = maxPlayerHp; // プレイヤーのHPを最大に設定
        currentPlayerMp = maxPlayerMp; // プレイヤーのMPを最大に設定

        // HPスライダーを初期化（UIに表示）
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxPlayerHp; // HPスライダーの最大値を設定
            healthSlider.value = playerHp; // 現在のHPスライダーの値を設定
        }

        // MPスライダーを初期化（UIに表示）
        if (mpSlider != null)
        {
            mpSlider.maxValue = maxPlayerMp; // MPスライダーの最大値を設定
            mpSlider.value = currentPlayerMp; // 現在のMPスライダーの値を設定
        }

        // MPを時間経過で増加させるコルーチンを開始
        StartCoroutine(IncreaseMpOverTime());
    }

    // 毎フレーム呼ばれる
    void Update()
    {
        // マウスの座標を取得
        mousePos = Input.mousePosition;

        // スクリーン座標をワールド座標に変換
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        // プレイヤーの位置をマウスの位置に設定
        transform.position = worldPos;

        // もし左クリックが押されたら
        if (Input.GetMouseButtonDown(0))
        {
            ReduceMp(); // MPを減らす
        }
    }

    // プレイヤーにダメージを与える関数
    public void AddDamage(int damage)
    {
        maxPlayerHp -= damage; // 最大HPをダメージ分減らす

        // HPが0未満にならないように制限
        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);

        // HPスライダーを更新
        if (healthSlider != null)
        {
            healthSlider.value = playerHp;
        }
    }

    // MPを減らす関数
    void ReduceMp()
    {
        // MPを減少させ、0未満にならないように制限
        currentPlayerMp = Mathf.Clamp(currentPlayerMp - mpDecreaseRate, 0, maxPlayerMp);

        // MPスライダーを更新
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // MPを増加させる関数
    void IncreaseMp()
    {
        // MPを増加させ、最大値を超えないように制限
        currentPlayerMp = Mathf.Min(currentPlayerMp + mpIncreaseAmount, maxPlayerMp);

        // MPスライダーを更新
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // MPを時間経過で増加させるコルーチン
    IEnumerator IncreaseMpOverTime()
    {
        while (true) // 無限ループでMPを増加させる
        {
            //if (!isPopUpWaiting)
            //{
                // MPを増加させる
                IncreaseMp();

                // もしMPが最大値未満で、ランダムな確率でポップアップを表示
                if (currentPlayerMp < maxPlayerMp && Random.value <= popUpChance)
                {
                    // ポップアップが表示されるチャンスが来たら
                    if (popUpController != null)
                    {
                        popUpController.StartRandomPopUpCoroutine(true); // ポップアップを開始
                        isPopUpWaiting = true;
                    }
                }
            //}
            // 指定した時間だけ待つ
            yield return new WaitForSeconds(mpIncreaseInterval);
            if (isPopUpWaiting)
            {
                yield return new WaitForSeconds(20f); // Wait for 20 seconds
                isPopUpWaiting = false; // Reset the flag to allow another pop-up
            }
            
        }
    }

}
