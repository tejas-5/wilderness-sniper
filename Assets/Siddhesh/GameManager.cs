using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject pausePanel; // Reference to the Pause Panel
    [SerializeField] private AudioClip gameClearSound;
    [SerializeField] private float volumeScale = 0.5f; // Default volume scale set to 50%
    private AudioSource audioSource;
    private bool isGameOver = false; // Flag to prevent multiple calls to GameOver
    public PopUpController popUpController;  // Reference to the PopUpController
    public GameObject errorCodePanel;  // Reference to the ErrorCode Panel

    public static GameManager Instance { get; private set; }
    private bool isPaused = false; // Flag to check if the game is paused

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Hide GameOverPanel initially
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false); // Hide the pause panel initially
        audioSource = GetComponent<AudioSource>();
        if (gameOverPanel == null)
        {
            Debug.LogWarning("GameOverPanel");
        }
    }

    public void GameOver()
    {
        if (isGameOver) return; // Exit if GameOver has already been triggered
        isGameOver = true; // Set the flag to true
        gameOverPanel.SetActive(true);
        // Check if popUpController is assigned before calling ClosePopUp
        if (popUpController != null)
        {
            Debug.Log("Closing PopUp");
            popUpController.ClosePopUp(); // Close the pop-up
        }
        else
        {
            Debug.LogWarning("PopUpController is not assigned in GameManager!");
        }
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

    public void ClosePopUp()
    {
        // Assuming the error code panel is a UI element (like a GameObject)
        if (errorCodePanel != null)
        {
            errorCodePanel.SetActive(false);
        }
    }

    public bool IsErrorScreenEnabled()
    {
        return errorCodePanel.gameObject.activeSelf;
    }

    public bool IsGameOverScreenEnabled()
    {
        return gameOverPanel.gameObject.activeSelf;
    }

    public bool IsPauseScreenEnabled()
    {
        return pausePanel.gameObject.activeSelf;
    }

    public bool AnyScreenEnabled()
    {
        return IsErrorScreenEnabled() || IsGameOverScreenEnabled() || IsPauseScreenEnabled();
    }

    // Method to handle pausing and unpausing the game
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (!isGameOver)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale before loading the main menu
        SceneManager.LoadScene("Main_Menu"); // Replace with your main menu scene name
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}