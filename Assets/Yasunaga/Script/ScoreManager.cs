using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;              // �X�R�A�ϐ�
    public Text scoreText;             // UI Text�R���|�[�l���g�Q�Ɨp

    void Start()
    {
        // �����X�R�A��\��
        UpdateScoreText();
    }

    // �X�R�A�����Z���郁�\�b�h
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();             // �X�R�A�\�����X�V
    }

    // �X�R�A�e�L�X�g���X�V���郁�\�b�h
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
