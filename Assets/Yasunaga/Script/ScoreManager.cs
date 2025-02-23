using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshProを使用するための追加

public class ScoreManager : MonoBehaviour
{
    public int score = 0;               // Score variable
    public TMP_Text scoreText;          // First UI Text (for displaying the score)
    public TMP_Text scoreText2;         // Second UI Text (for displaying the same score)

    void Start()
    {
        // Initialize the score display
        UpdateScoreText();
    }

    // Method to add points to the score
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();              // Update both Text elements
    }

    // Method to update both UI Text elements with the current score
    void UpdateScoreText()
    {
        scoreText.text = score.ToString();
        scoreText2.text = "Score: " + score.ToString();  // Update second score display
    }
}
