using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UIを使う場合


public class PlayerController : MonoBehaviour
{
    //座標用の変数
    Vector3 mousePos, worldPos;

    [SerializeField] int maxPlayerHp = 200;
    private int playerHp;
    public Slider healthSlider; // UIのスライダーで体力を表示

    [SerializeField] int maxPlayerMp = 100;
    [SerializeField] int downMp = 10;
    public Slider mpSlider;

    void Start()
    {
        playerHp = maxPlayerHp;

        // UIのスライダーを初期化（オプション）
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

        if (mpSlider != null)
        {
            mpSlider.value = maxPlayerMp;
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
        // MPを減少
        maxPlayerMp = Mathf.Max(0, maxPlayerMp - downMp);
    }


}

