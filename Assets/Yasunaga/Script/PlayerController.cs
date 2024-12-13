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
    }

    void Update()
    {
        //マウス座標の取得
        mousePos = Input.mousePosition;
        //スクリーン座標をワールド座標に変換
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        //ワールド座標を自身の座標に設定
        transform.position = worldPos;
        Debug.Log(maxPlayerHp);

        if (Input.GetMouseButtonDown(0)) // 0は左クリック
        {
            ReduceMp();
        }

        // 時間経過でMPを減少
        ReduceMpOverTime();
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
        currentPlayerMp = Mathf.Clamp(currentPlayerMp + upMp, 0, maxPlayerMp);

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


}

