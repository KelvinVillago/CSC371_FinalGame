using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _pauseScreen;
    [SerializeField] GameObject _controls;

    bool _isPaused = false;
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Play()
    {
        Time.timeScale = 1f;
        _isPaused = false;
        SceneManager.LoadScene("Level-01");
    }

    public void Menu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Controls()
    {
        if(_controls != null)
        {
            _controls.SetActive(true);
        }
    }

    public void CloseControls()
    {
        if(_controls != null)
        {
            _controls.SetActive(false);
        }
    }

    void Awake()
    {
        if(_pauseScreen != null)
        {
            _pauseScreen.SetActive(false);
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && _pauseScreen != null)
        {
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        _pauseScreen.SetActive(true);
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        _pauseScreen.SetActive(false);
    }
}
