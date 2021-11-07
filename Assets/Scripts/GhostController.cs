using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour, IForm
{
    private GameObject player;
    private PlayerController playerController;

    private bool moveReady = true;
    private float moveTimer = 0f;
    private int moveCount = 0;

    void Start()
    {
        player = transform.parent.gameObject;
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        player.transform.position = Vector3.MoveTowards(player.transform.position, playerController.moveDest.position, playerController.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, playerController.moveDest.position) == 0f)
        {
            if (moveTimer <= 0f) moveReady = true;
            else moveTimer -= Time.deltaTime;
        }

        if (moveReady)
        {
            if (Input.GetKeyDown("space"))
            {
                playerController.SwapIn();
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                MakeMove(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f));
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                MakeMove(new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f));
            }
        }
    }

    void MakeMove(Vector3 dir)
    {
        if (moveCount >= 3)
        { //If you did 3 moves
            Object.Destroy(this.gameObject);
        }
        else if (!Physics2D.OverlapCircle(playerController.moveDest.position + dir, .2f, playerController.stopsMove))
        {
            playerController.moveDest.position += dir;
            moveReady = false;
            moveTimer = playerController.moveDelay;
            moveCount++;
        }
    }

    public void Wake()
    {
        this.enabled = true;
        moveReady = false;
        moveTimer = playerController.moveDelay * 3;
        moveCount = 0;
    }

    public void Sleep()
    {
        this.enabled = false;
        moveCount = 0;
    }
}
