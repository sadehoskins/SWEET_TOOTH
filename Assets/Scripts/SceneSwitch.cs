using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void LoadMain()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void LoadWin()
    {
        SceneManager.LoadScene(3);
        Time.timeScale = 1;
    }

    public void LoadLose()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
