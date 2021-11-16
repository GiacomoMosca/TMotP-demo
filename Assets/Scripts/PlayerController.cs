using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject ghost;
    public GameObject marker;
    private SpriteRenderer ghostSprite;
    private SpriteRenderer markerSprite;

    private GameObject form = null;
    private GameObject swappable = null;
    private List<string> availableForms;

    public float moveSpeed;
    public float moveDelay;
    public Vector3 moveDest;
    public LayerMask stopsMove;
    public LayerMask pushable;

    private GameObject form = null;
    private GameObject swappable = null;
    private List<string> availableForms;

    [SerializeField]
    private GameObject ghost;
    [SerializeField]
    private GameObject marker;
    private SpriteRenderer ghostSprite;
    private SpriteRenderer markerSprite;

    public LayerMask button;

    public Animator transition;

    void Start()
    {
        ghostSprite = ghost.GetComponent<SpriteRenderer>();
        markerSprite = marker.GetComponent<SpriteRenderer>();
        markerSprite.enabled = false;
        moveDest = transform.position;
        availableForms = new List<string> { "Vase" };
        ghost.GetComponent<IForm>().Wake();
    }

    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Reload level
            return;
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
        ghostSprite.enabled = true;
        ghost.GetComponent<IForm>().Wake();
        form = null;
        swappable = null;
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
        ghostSprite.enabled = true;
        ghost.GetComponent<IForm>().Wake();
        form = null;
        swappable = null;
    }

    public void GameOver()
    {
        markerSprite.enabled = false;
        form = null;
        swappable = null;
    }
}