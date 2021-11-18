using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void Pause()
    {
        this.gameObject.SetActive(true);
        Time.timeScale = 0f;
        BGMController.instance.Pause();
        SFXController.instance.Pause();
    }

    public void Resume()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
        BGMController.instance.Unpause();
        SFXController.instance.Unpause();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
