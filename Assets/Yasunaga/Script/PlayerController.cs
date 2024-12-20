using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UIのためのライブラリ

public class PlayerController : MonoBehaviour
{
    // マウス位置とワールド位置を保存するための変数
    Vector3 mousePos, worldPos;

    [SerializeField] int maxPlayerHp = 100; // 最大HP
    private int playerHp; // 現在のHP
    public Slider healthSlider; // UIのHPバー

    [SerializeField] float maxPlayerMp = 100f; // 最大MP
    [SerializeField] float mpDecreaseRate = 5f; // MPが減る速度
    private float currentPlayerMp; // 現在のMP
    public Slider mpSlider; // UIのMPバー

    public PopUpController popUpController; // ポップアップを表示するコントローラー
    [SerializeField] float mpIncreaseInterval = 1f; // MPが回復する時間間隔
    [SerializeField] int mpIncreaseAmount = 1; // 1回のMP回復量
    [SerializeField] float popUpChance = 0.1f; // ポップアップが表示される確率（0.0 - 1.0）

    private bool isPopUpWaiting = false; // ポップアップが待機中かどうかのフラグ

    // ゲーム開始時に1回だけ実行される
    void Start()
    {
        playerHp = maxPlayerHp; // プレイヤーのHPを最大値に設定
        currentPlayerMp = maxPlayerMp; // プレイヤーのMPを最大値に設定

        // HPバーの設定
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxPlayerHp; // HPバーの最大値を設定
            healthSlider.value = playerHp; // 現在のHPをバーに設定
        }

        // MPバーの設定
        if (mpSlider != null)
        {
            mpSlider.maxValue = maxPlayerMp; // MPバーの最大値を設定
            mpSlider.value = currentPlayerMp; // 現在のMPをバーに設定
        }

        // 一定時間ごとにMPを回復する処理を開始
        StartCoroutine(IncreaseMpOverTime());
    }

    // 毎フレーム実行される
    void Update()
    {
        // マウスの位置を取得
        mousePos = Input.mousePosition;

        // マウスのスクリーン座標をワールド座標に変換
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        // プレイヤーの位置をマウスの位置に合わせる
        transform.position = worldPos;

        // マウスの左クリックでMPを減らす
        if (Input.GetMouseButtonDown(0))
        {
            ReduceMp(); // MPを減らす
        }
    }

    // ダメージを受けたときに呼ばれる
    public void AddDamage(int damage)
    {
        maxPlayerHp -= damage; // HPを減らす

        // HPが0以下にならないように制限
        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);

        // HPバーを更新
        if (healthSlider != null)
        {
            healthSlider.value = playerHp;
        }
    }

    // MPを減らす処理
    void ReduceMp()
    {
        // MPが0以下にならないように制限
        currentPlayerMp = Mathf.Clamp(currentPlayerMp - mpDecreaseRate, 0, maxPlayerMp);

        // MPバーを更新
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // MPを回復する処理
    void IncreaseMp()
    {
        // MPが最大値を超えないように制限
        currentPlayerMp = Mathf.Min(currentPlayerMp + mpIncreaseAmount, maxPlayerMp);

        // MPバーを更新
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    // 一定時間ごとにMPを回復するコルーチン
    IEnumerator IncreaseMpOverTime()
    {
        Debug.Log("MP Recovery Coroutine Started!");
        while (true) // 無限ループでMPを回復
        {
            // MPを回復
            IncreaseMp();
            yield return new WaitForSeconds(mpIncreaseInterval);

            // MPが最大ではなく、ポップアップの確率に基づいてポップアップを表示
            if (currentPlayerMp < maxPlayerMp && Random.value <= popUpChance)
            {
                // ポップアップを表示
                if (popUpController != null)
                {
                    popUpController.StartRandomPopUpCoroutine(true); // ポップアップを表示
                    isPopUpWaiting = true;
                }
            }

            // 次の回復まで待機
            yield return new WaitForSeconds(mpIncreaseInterval);

            // ポップアップ待機中のフラグが立っていれば、20秒後にリセット
            if (isPopUpWaiting)
            {
                yield return new WaitForSeconds(20f); // 20秒待つ
                isPopUpWaiting = false; // フラグをリセットして、次のポップアップを許可
            }
        }
    }
}