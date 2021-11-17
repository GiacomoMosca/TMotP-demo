using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableController : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;

    private Vector3 moveDest;
    private bool atDestination = false;
    private bool fellPit = false;

    private bool onButton = false;
    private LayerMask layerButton;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        moveDest = transform.position;
        layerButton += LayerMask.GetMask("Button");
    }


    void Update()
    {
        atDestination = (Vector3.Distance(transform.position, moveDest) == 0f);

        if (!atDestination)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveDest, playerController.moveSpeed * Time.deltaTime);
        }
        else if (fellPit)
        {
            Object.Destroy(this.gameObject);
            return;
        }
    }

    void OnEnable()
    {
        onButton = false;
        moveDest = transform.position;
    }

    void OnDisable()
    {
        if (Physics2D.OverlapCircle(moveDest, .2f, layerButton)) onButton = true;
    }

    public bool TryMove(Vector3 dir)
    {
        if (!atDestination || onButton) return false;

        Collider2D collided = Physics2D.OverlapCircle(moveDest + dir, .2f);
        bool collidedButton = false;
        bool collidedPit = false;

        if (collided)
        {
            collidedButton = collided.tag == "Button";
            collidedPit = collided.tag == "Pit";
            if (!collidedButton && !collidedPit) return false;
        }

        if (collided && collidedButton) onButton = true;
        if (collided && collidedPit) fellPit = true;
        moveDest += dir;
        return true;
    }
}
