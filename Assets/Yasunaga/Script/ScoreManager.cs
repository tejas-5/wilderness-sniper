using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;              // Score variable
    public Text scoreText;             // First UI Text (for displaying the score)
    public Text scoreText2;            // Second UI Text (for displaying the same score)

    void Start()
    {
        // Initialize the score display
        UpdateScoreText();
    }

    // Method to add points to the score
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();             // Update both Text elements
    }

    // Method to update both UI Text elements with the current score
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
        scoreText2.text = "Score: " + score;  // Update second score display
    }
}
