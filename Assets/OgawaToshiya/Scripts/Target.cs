using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{    
    Vector3 mousePos, worldPos;     // 座標用の変数
    public int maxBullets = 15;     // 弾数15 
    private int currentBullets;     // 残っている弾

    // Start is called before the first frame update
    void Start()
    {
        currentBullets = maxBullets;
        UpdateClickDisplay();
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

        // 残弾数がある場合オブジェクトを消せる
        if (Input.GetMouseButtonDown(0) && currentBullets > 0)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            currentBullets--;

            if (hit.collider != null && hit.collider.gameObject.CompareTag("DestroyableObject"))
            {
                Destroy(hit.collider.gameObject);
            }
            UpdateClickDisplay();
        }
    }

    void UpdateClickDisplay()
    {
        if (currentBullets > 0)
        {
            Debug.Log("残りクリック回数: " + currentBullets);
        }
        else
        {
            Debug.Log("クリック回数が0になりました。これ以上クリックできません。");
        }
    }
}
