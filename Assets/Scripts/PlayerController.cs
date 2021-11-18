using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject ghost;
    [SerializeField]
    private GameObject marker;
    [SerializeField]
    private ParticleSystem deathParticles;
    private SpriteRenderer ghostSprite;
    private SpriteRenderer markerSprite;

    private GameObject form = null;
    private GameObject swappable = null;
    private List<string> availableForms;
    private bool isInitialized = false;

    public float moveSpeed;
    public float moveDelay;
    public Vector3 moveDest;
    public LayerMask stopsMove;
    public LayerMask pushable;

    public LayerMask buttonMask;
	
    public bool isFrozen = false;
    public bool isPaused = false;

    void Start()
    {
        ghostSprite = ghost.GetComponent<SpriteRenderer>();
        markerSprite = marker.GetComponent<SpriteRenderer>();
        markerSprite.enabled = false;
        moveDest = transform.position;
        availableForms = new List<string> { "Vase", "Mummy" }; 
    }

    void Update()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            ghost.GetComponent<IForm>().Wake();
        }

        if (Input.GetKeyDown("escape"))
        {
            if (isPaused)
            {
                PauseMenu.instance.Resume();
                isPaused = false;
            }
            else
            {
                PauseMenu.instance.Pause();
                isPaused = true;
            }
        }

        if (Input.GetKeyDown("r") && Time.timeScale != 0f)
        {
            LevelLoader.instance.RestartLevel();
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        swappable = null;
        markerSprite.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (availableForms.Contains(other.tag))
        {
            swappable = other.gameObject;
            markerSprite.enabled = true;
        }
        else if (other.tag == "NextLevel")
        {
            SFXController.instance.PlaySound("nextRoom");
            LevelLoader.instance.LoadNextLevel();
        }
    }

    public void SwapIn()
    {
        if (form != null || swappable == null) return;
		
        markerSprite.enabled = false;
        form = swappable;
        form.transform.parent = transform;
        ghostSprite.enabled = false;
        ghost.GetComponent<IForm>().Sleep();
        form.GetComponent<IForm>().Wake();
        SFXController.instance.PlaySound("transformIn");
    }

    public void SwapOut()
    {
        if (form == null) return;
		
        markerSprite.enabled = true;
        form.transform.parent = null;
        form.GetComponent<IForm>().Sleep();
        swappable = form;
        ghostSprite.enabled = true;
        ghost.GetComponent<IForm>().Wake();
        form = null;
        SFXController.instance.PlaySound("transformOut");
    }

    public void FormDestroyed()
    {
        deathParticles.Play();
        ghostSprite.enabled = true;
        ghost.GetComponent<IForm>().Wake();
        form = null;
        swappable = null;
    }

    public void GameOver()
    {
        deathParticles.Play();
        SFXController.instance.PlaySound("death");
        markerSprite.enabled = false;
        form = null;
        swappable = null;
        UIManager.instance.ShowPrompt();
    }
}