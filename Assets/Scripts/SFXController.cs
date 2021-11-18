using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    public static SFXController instance;
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

    public static AudioClip sfxDeath, sfxDialogue, sfxDestroyVase, sfxGhostStep, sfxMummyDeath,
        sfxMummyStep, sfxNextRoom, sfxRestart, sfxStart, sfxTransformIn, sfxTransformOut, sfxVaseRoll;

    private AudioSource audioSrc;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        sfxDeath = Resources.Load<AudioClip>("death");
        sfxDialogue = Resources.Load<AudioClip>("dialogue");
        sfxDestroyVase = Resources.Load<AudioClip>("destroyVase");
        sfxGhostStep = Resources.Load<AudioClip>("ghostStep");
        sfxMummyDeath = Resources.Load<AudioClip>("mummyDeath");
        sfxMummyStep = Resources.Load<AudioClip>("mummyStep");
        sfxNextRoom = Resources.Load<AudioClip>("nextRoom");
        sfxRestart = Resources.Load<AudioClip>("restart");
        sfxStart = Resources.Load<AudioClip>("start");
        sfxTransformIn = Resources.Load<AudioClip>("transformIn");
        sfxTransformOut = Resources.Load<AudioClip>("transformOut");
        sfxVaseRoll = Resources.Load<AudioClip>("vaseRoll");
    }

    public void PlaySound(string clip)
    {
        switch (clip) 
        {
            case "death":
                audioSrc.PlayOneShot(sfxDeath);
                break;
            case "dialogue":
                audioSrc.PlayOneShot(sfxDialogue);
                break;
            case "destroyVase":
                audioSrc.PlayOneShot(sfxDestroyVase);
                break;
            case "ghostStep":
                audioSrc.PlayOneShot(sfxGhostStep);
                break;
            case "mummyDeath":
                audioSrc.PlayOneShot(sfxMummyDeath);
                break;
            case "mummyStep":
                audioSrc.PlayOneShot(sfxMummyStep);
                break;
            case "nextRoom":
                audioSrc.PlayOneShot(sfxNextRoom);
                break;
            case "restart":
                audioSrc.PlayOneShot(sfxRestart);
                break;
            case "start":
                audioSrc.PlayOneShot(sfxStart);
                break;
            case "transformIn":
                audioSrc.PlayOneShot(sfxTransformIn);
                break;
            case "transformOut":
                audioSrc.PlayOneShot(sfxTransformOut);
                break;
            case "vaseRoll":
                audioSrc.loop = true;
                audioSrc.clip = sfxVaseRoll;
                audioSrc.Play();
                break;
        }
    }

    public void StopSound()
    {
        audioSrc.Stop();
        audioSrc.loop = false;
        audioSrc.clip = null;
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
