using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject ghost;
    private GameObject form = null;
    private GameObject swappable = null;
    private List<string> availableForms;

    public float moveSpeed;
    public float moveDelay;
    public Transform moveDest;
    public LayerMask stopsMove;

    void Start()
    {
        moveDest.parent = null;
        availableForms = new List<string> { "Vase" };
    }

    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("Main"); //Reload level
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        swappable = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (availableForms.Contains(other.tag))
        {
            swappable = other.gameObject;
        }
    }

    public void SwapIn()
    {
        if (form != null || swappable == null) return;
        form = swappable;
        form.transform.parent = transform;
        ghost.GetComponent<SpriteRenderer>().enabled = false;
        ghost.GetComponent<IForm>().Sleep();
        form.GetComponent<IForm>().Wake();
    }

    public void SwapOut()
    {
        if (form != null)
        {
            form.transform.parent = null;
            form.GetComponent<IForm>().Sleep();
            swappable = form;
        }
        ghost.GetComponent<SpriteRenderer>().enabled = true;
        ghost.GetComponent<IForm>().Wake();
        form = null;
    }
}