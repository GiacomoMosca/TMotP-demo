using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    private PlayerController playerController;

    private Vector3 moveDest;
    private bool moving;

    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        moveDest = transform.position;
        moving = false;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveDest, playerController.moveSpeed * Time.deltaTime);
        moving = transform.position != moveDest;
    }

    public bool TryMove(Vector3 dir)
    {
        if (moving) return false;
        if (Physics2D.OverlapCircle(transform.position + dir, .2f)) return false;
        moveDest += dir;
        return true;
    }
}
