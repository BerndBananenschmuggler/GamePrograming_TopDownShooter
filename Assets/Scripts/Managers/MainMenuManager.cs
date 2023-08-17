using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void ButtonPlay_Clicked()
    {
        SceneManager.LoadScene("Game");
    }
    public void ButtonQuit_Clicked()
    {
        Application.Quit();
    }
}
