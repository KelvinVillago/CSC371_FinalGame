using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _pauseScreen;
    [SerializeField] GameObject _controls;
    [SerializeField] GameObject _credits;
    public AudioClip clickSound;
    private AudioSource _audioSource;

    bool _isPaused = false;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void QuitGame()
    {
        _audioSource.PlayOneShot(clickSound);
        Application.Quit();
    }

    public void Play()
    {
        _audioSource.PlayOneShot(clickSound);
        Time.timeScale = 1f;
        _isPaused = false;
        SceneManager.LoadScene("Level-01");
    }

    public void Menu()
    {
        _audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("StartMenu");
    }

    public void Credits()
    {
        _audioSource.PlayOneShot(clickSound);
        if(_credits != null)
        {
            _credits.SetActive(true);
        }
    }

    public void CloseCredits()
    {
        _audioSource.PlayOneShot(clickSound);
        if(_credits != null)
        {
            _credits.SetActive(false);
        }
    }

    public void Controls()
    {
        _audioSource.PlayOneShot(clickSound);
        if(_controls != null)
        {
            _controls.SetActive(true);
        }
    }

    public void CloseControls()
    {
        _audioSource.PlayOneShot(clickSound);
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
