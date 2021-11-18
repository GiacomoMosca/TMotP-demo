using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;

    void Awake()
    {
        instance = this;
    }

    public Animator transition;
    public float transitionTime = 0.3f;

    private bool hasLoaded;
    private PlayerController playerController;

    void Start()
    {
        hasLoaded = false;
        playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!LevelData.hasLoaded && !hasLoaded)
        {
            LevelData.hasLoaded = true;
            hasLoaded = true;
            if (playerController)
            {
                FreezePlayer();
                StartCoroutine(DisplayDialogue());
            }
        }
        hasLoaded = true;
    }

    IEnumerator DisplayDialogue()
    {
        yield return new WaitForSeconds(1f);
        DialogueManager.instance.StartDialogue();
    }

    public void LoadNextLevel()
    {
        LevelData.hasLoaded = false;
        StopAllCoroutines();
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1, true));
    }

    public void RestartLevel()
    {
        SFXController.instance.PlaySound("restart");
        StopAllCoroutines();
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex, false));
    }

    IEnumerator LoadLevel (int levelIndex, bool isNextLevel)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        if (isNextLevel)
        {
            yield return BGMController.instance.FadeOut();
            BGMController.instance.DestroySource();
        }

        SceneManager.LoadScene(levelIndex);
    }

    public void FreezePlayer()
    {
        playerController.isFrozen = true;
    }

    public void UnfreezePlayer()
    {
        playerController.isFrozen = false;
    }
}
