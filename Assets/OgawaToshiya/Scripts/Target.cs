using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // 座標用の変数
    Vector3 mousePos, worldPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //マウス座標の取得
        mousePos = Input.mousePosition;
        //スクリーン座標をワールド座標に変換
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        //ワールド座標を自身の座標に設定
        transform.position = worldPos;
    }
}
