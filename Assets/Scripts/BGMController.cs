using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public static BGMController instance;

    [SerializeField]
    private float waitTime;
    [SerializeField]
    private float fadeTime;
    private AudioSource audioSrc;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        StartCoroutine(StartMusic());
    }

    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(waitTime);
        audioSrc.Play();
    }

    public IEnumerator FadeOut()
    {
        float startVolume = audioSrc.volume;

        while (audioSrc.volume > 0)
        {
            audioSrc.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSrc.Stop();
        audioSrc.volume = startVolume;
    }

    public void DestroySource()
    {
        Destroy(this.gameObject);
        instance = null;
    }

    public void Pause()
    {
        audioSrc.Pause();
    }

    public void Unpause()
    {
        audioSrc.UnPause();
    }    
}
