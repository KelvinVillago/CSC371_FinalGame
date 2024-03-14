using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;

    // Haungs Mode
    public TextMeshProUGUI haungsText;
    public Button haungsModeButton;
    public GameObject genCoinButton;
    private bool haungsModeEnabled = false;

    void Start()
    {
        UpdateButtonText();
        ToggleGenCoinVisibility();
    }

    public void PauseGame()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;
    }

    public void ToggleHaungsMode()
    {
        haungsModeEnabled = !haungsModeEnabled;
        UpdateButtonText();
        ToggleGenCoinVisibility();
    }

    void UpdateButtonText()
    {
        haungsText.text = haungsModeEnabled ? "Disable Haungs Mode" : "Enable Haungs Mode";
    }

    void ToggleGenCoinVisibility()
    {
        if (genCoinButton != null)
        {
            genCoinButton.SetActive(haungsModeEnabled);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
