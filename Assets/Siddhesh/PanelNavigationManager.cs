using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PanelNavigationManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject[] panels; // Array of panels to navigate
    [SerializeField] private GameObject mainScreen; // Main screen to return to when closing

    [Header("Scene Settings")]
    [SerializeField] private SceneAsset gameScene; // Drag the scene asset here

    private int currentPanelIndex = 0; // Tracks the current panel being displayed

    private void Start()
    {
        // Ensure only the main screen is active at the start
        ShowMainMenu();
    }

    // Navigation Buttons
    public void GoToPreviousPanel()
    {
        if (currentPanelIndex > 0)
        {
            ShowPanel(currentPanelIndex - 1);
        }
        else
        {
            Debug.Log("Already at the first panel!");
        }
    }

    public void GoToNextPanel()
    {
        if (currentPanelIndex < panels.Length - 1)
        {
            ShowPanel(currentPanelIndex + 1);
        }
        else
        {
            Debug.Log("Already at the last panel!");
        }
    }

    public void ClosePanels()
    {
        // Hide all panels and activate the main screen
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(true);
        }
        currentPanelIndex = 0; // Reset to the first panel
    }

    // Main Menu Buttons
    public void StartGame()
    {
        if (gameScene != null)
        {
            // Load the scene by its name
            string sceneName = gameScene.name;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Game scene is not assigned in the PanelNavigationManager script!");
        }
    }

    public void ExitGame()
    {
        // Quit the application
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    public void OpenHelp()
    {
        // Show the first panel
        ShowPanel(0);
    }

    // Helper Methods
    private void ShowMainMenu()
    {
        // Hide all panels and display the main screen
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(true);
        }
    }

    private void ShowPanel(int index)
    {
        // Deactivate all panels
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        // Activate the specified panel
        if (index >= 0 && index < panels.Length)
        {
            panels[index].SetActive(true);
            currentPanelIndex = index;
        }
        else
        {
            Debug.LogWarning("Invalid panel index!");
        }

        // Deactivate the main screen
        if (mainScreen != null)
        {
            mainScreen.SetActive(false);
        }
    }
}
