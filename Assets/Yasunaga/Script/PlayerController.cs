using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UIを使う場合


public class PlayerController : MonoBehaviour
{
    //座標用の変数
    Vector3 mousePos, worldPos;

    [SerializeField] int maxPlayerHp = 100;
    private int playerHp;
    public Slider healthSlider; // UIのスライダーで体力を表示

    [SerializeField] float maxPlayerMp = 100f;
    [SerializeField] float upMp = 10f; // MP増加量
    [SerializeField] float mpDecreaseRate = 5f; // 毎秒減少するMP量
    private float currentPlayerMp; // 現在のMP
    public Slider mpSlider;

    public PopUpController popUpController;
    [SerializeField] float mpIncreaseInterval = 1f; // MPが増加する間隔（秒）
    [SerializeField] int mpIncreaseAmount = 1; // 1回の増加量
    [SerializeField] float popUpChance = 0.5f; // ポップアップが表示される確率（0.0 - 1.0）

    void Start()
    {
        playerHp = maxPlayerHp;
        currentPlayerMp = maxPlayerMp; // MPは最大値で開始

        // UIのスライダーを初期化（オプション）
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
        // MPが時間経過で増加するコルーチンを開始
        StartCoroutine(IncreaseMpOverTime());
        
    }

    void Update()
    {
        //マウス座標の取得
        mousePos = Input.mousePosition;
        //スクリーン座標をワールド座標に変換
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        //ワールド座標を自身の座標に設定
        transform.position = worldPos;
        //Debug.Log(maxPlayerHp);
        //Debug.Log(maxPlayerMp);

        if (Input.GetMouseButtonDown(0)) // 0は左クリック
        {
            ReduceMp();
        }
    }

    public void AddDamage(int damage)
    {
        maxPlayerHp -= damage;

        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);

        // UIのスライダーを更新
        if (healthSlider != null)
        {
            healthSlider.value = playerHp;
        }
    }

    void ReduceMp()
    {
        // MPを増加
        currentPlayerMp = Mathf.Clamp(currentPlayerMp - mpDecreaseRate, 0, maxPlayerMp);

        // スライダーの値を更新
        if (mpSlider != null)
        {
            mpSlider.value = currentPlayerMp;
        }
    }

    void ReduceMpOverTime()
    {
        // 時間経過に基づいてMPを減少
        currentPlayerMp = Mathf.Clamp(currentPlayerMp - mpDecreaseRate * Time.deltaTime, 0, maxPlayerMp);

        // スライダーの値を更新
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
    // コルーチンでMPを時間経過で増加させる
    IEnumerator IncreaseMpOverTime()
    {
        while (true) // 無限ループでMPを増加し続ける
        {
            // MP増加処理
            IncreaseMp();

            // チャンスの確率でポップアップ表示
            if (currentPlayerMp < maxPlayerMp && Random.value <= popUpChance) // チャンスの確率でポップアップ表示
            {
                if (popUpController != null)
                {
                    popUpController.StartRandomPopUpCoroutine(true); // Start the pop-up coroutine here
                }
            }

            // 指定した間隔だけ待機
            yield return new WaitForSeconds(mpIncreaseInterval);
        }
    }
    void ShowPopUp()
    {
        if (popUpController != null)
        {
            // ポップアップを表示するための関数を呼び出す
            popUpController.ShowPopUp();
        }
    }
}