using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmController : MonoBehaviour
{
    [SerializeField] int armHp = 10;
    [SerializeField] int hitDamage = 1;
    [SerializeField] int scoreValue = 100; // Score value when destroyed
    [SerializeField] AudioClip destructionSound; // Sound to play when destroyed
    private ScoreManager scoreManager;
    private AudioSource audioSource; // Reference to AudioSource
    private bool isDestroyed = false; // Flag to ensure the sound is played once

    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component on this GameObject
    }

    void Update()
    {
        if (armHp == 0 && !isDestroyed) // Check if the arm is not destroyed yet
        {
            Die();
        }
    }

    void OnMouseDown()
    {
        if (!isDestroyed) // Only allow damage if the arm is not destroyed
        {
            armHp -= hitDamage;
        }
    }

    private void Die()
    {
        // Set the flag to prevent further destruction
        isDestroyed = true;

        // Add score
        scoreManager.AddScore(scoreValue);

        // Play the destruction sound
        if (audioSource != null && destructionSound != null)
        {
            audioSource.PlayOneShot(destructionSound);
        }

        // Destroy the GameObject after the sound finishes playing
        Destroy(gameObject, destructionSound.length); // Destroy after the sound finishes playing
    }
}
