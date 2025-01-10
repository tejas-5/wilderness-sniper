using UnityEngine;
using UnityEngine.SceneManagement; // To reload the scene
using UnityEngine.UI; // To interact with UI elements

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void Start()
    {
        // Hide GameOverPanel initially
        gameOverPanel.SetActive(false);
    }

    public void GameOver()
    {
        // Display the GameOverPanel when the player loses
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        // Restart the current scene (reload the level)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        // Quit the application (useful in a build)
        Application.Quit();
    }
}
