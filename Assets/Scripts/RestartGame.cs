using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void RestartGame2()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
