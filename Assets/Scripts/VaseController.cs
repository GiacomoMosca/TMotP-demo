using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseController : MonoBehaviour, IForm
{
    private GameObject player;
    private PlayerController playerController;

    private bool moveReady = true;
    private float moveTimer = 0f;
    private int moveCount = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        this.enabled = false;
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
                playerController.SwapOut();
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
            playerController.SwapOut();
            Object.Destroy(this.gameObject);
        }
        else if (!Physics2D.OverlapCircle(playerController.moveDest.position + dir, .2f))
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
        moveTimer = playerController.moveDelay;
        moveCount = 0;
    }

    public void Sleep()
    {
        this.enabled = false;
        moveCount = 0;
    }

}
