using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableController : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;

    private Vector3 moveDest;
    private bool isMoving = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        moveDest = transform.position;
    }

    void OnEnable()
    {
        moveDest = transform.position;
    }

    void Update()
    {
        isMoving = (Vector3.Distance(transform.position, moveDest) != 0f);
        if (isMoving) transform.position = Vector3.MoveTowards(transform.position, moveDest, playerController.moveSpeed * Time.deltaTime);
    }

    public bool TryMove(Vector3 dir)
    {
        if (isMoving) return false;
        if (Physics2D.OverlapCircle(transform.position + dir, .2f)) return false;
        moveDest += dir;
        return true;
    }
}
