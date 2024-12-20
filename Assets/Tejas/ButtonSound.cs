using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public Button myButton;  // Reference to the button
    public AudioClip clickSound;  // The sound to play on button click
    private AudioSource audioSource;  // Reference to the AudioSource component

    void Start()
    {
        // Get the AudioSource component on this GameObject (or assign one directly)
        audioSource = GetComponent<AudioSource>();

        // Ensure the button is assigned
        if (myButton != null)
        {
            myButton.onClick.AddListener(PlayButtonSound);
        }
    }

    // Method to play the sound
    void PlayButtonSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
