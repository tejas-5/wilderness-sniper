using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;              // スコア変数
    public Text scoreText;             // UI Textコンポーネント参照用

    void Start()
    {
        // 初期スコアを表示
        UpdateScoreText();
    }

    // スコアを加算するメソッド
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();             // スコア表示を更新
    }

    // スコアテキストを更新するメソッド
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
