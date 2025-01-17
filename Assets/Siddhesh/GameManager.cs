using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    [SerializeField] private AudioClip gameClearSound;
    [SerializeField] private float volumeScale = 0.5f; // Default volume scale set to 50%
    private AudioSource audioSource;

    private bool isGameOver = false; // Flag to prevent multiple calls to GameOver

    private void Start()
    {
        // Hide GameOverPanel initially
        gameOverPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void GameOver()
    {
        if (isGameOver) return; // Exit if GameOver has already been triggered

        isGameOver = true; // Set the flag to true
        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;

        // Play the gameClearSound with adjustable volume
        if (gameClearSound != null)
        {
            audioSource.PlayOneShot(gameClearSound, volumeScale);
        }
        else
        {
            Debug.LogWarning("GameClearSound is not assigned in the GameManager script!");
        }
    }

    public void RestartGame()
    {
        // Reset the flag when restarting the game
        isGameOver = false;

        Time.timeScale = 1f;

        // Restart the current scene (reload the level)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        // Quit the application (useful in a build)
        Application.Quit();
    }
}
