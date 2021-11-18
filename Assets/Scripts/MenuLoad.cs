using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoad : MonoBehaviour
{
    public void PlayGame()
    {
        SFXController.instance.PlaySound("start");
        LevelLoader.instance.LoadNextLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
