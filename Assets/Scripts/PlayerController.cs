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

    public LayerMask button;

    public Animator transition;
	
	private DialogueTrigger dialogueTrigger;

    void Start()
    {
        ghostSprite = ghost.GetComponent<SpriteRenderer>();
        markerSprite = marker.GetComponent<SpriteRenderer>();
        markerSprite.enabled = false;
        moveDest = transform.position;
        availableForms = new List<string> { "Vase" }; 
		dialogueTrigger = FindObjectOfType<DialogueTrigger>();
    }

    void Update()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            ghost.GetComponent<IForm>().Wake();
			dialogueTrigger.TriggerDialogue();
        }

        if (Input.GetKeyDown("r"))
        {
            LevelLoader.instance.RestartLevel();
            return;

        if (Input.GetKeyDown("x"))
        {
            dialogueTrigger.TriggerDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        swappable = null;
        markerSprite.enabled = false;
    }

    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (availableForms.Contains(other.tag))
        {
            swappable = other.gameObject;
            markerSprite.enabled = true;
        }
        else if (other.tag == "NextLevel")
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(1);
        
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        markerSprite.enabled = false;
        form = null;
        swappable = null;
    }
}