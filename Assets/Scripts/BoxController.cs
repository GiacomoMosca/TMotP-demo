using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxController : MonoBehaviour
{
    private PlayerController playerController;

    private Vector3 moveDest;
    private bool moving;
    private List<string> availableForms;

    [Header("push button")]
    public UnityEvent PushButton;

    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        moveDest = transform.position;
        moving = false;
        availableForms = new List<string> { "Button" };
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveDest, playerController.moveSpeed * Time.deltaTime);
        moving = transform.position != moveDest;
    }

    public bool TryMove(Vector3 dir)
    {
        if (moving) return false;
        if (Physics2D.OverlapCircle(transform.position + dir, .2f) && (Physics2D.OverlapCircle(transform.position + dir, .2f, playerController.pushable) || Physics2D.OverlapCircle(transform.position + dir, .2f, playerController.stopsMove))) return false;
        moveDest += dir;
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (availableForms.Contains(other.tag))
        {
            Debug.Log("Invoke");
            PushButton.Invoke();
        }
    }
}
