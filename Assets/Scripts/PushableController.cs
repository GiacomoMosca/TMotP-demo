using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PushableController : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;

    private Vector3 moveDest;
    private bool atDestination = false;

    private bool onButton = false;
    private LayerMask layerButton;

    [SerializeField]
    private Tile filledTile;
    private Tilemap pitMap;
    private Tilemap bgMap;
    private bool fellPit;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        moveDest = transform.position;
        layerButton += LayerMask.GetMask("Button");
        pitMap = GameObject.FindGameObjectWithTag("Pit").GetComponent<Tilemap>();
        bgMap = GameObject.FindGameObjectWithTag("Background").GetComponent<Tilemap>();
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
            if (this.tag == "Box")
            {
                pitMap.SetTile(pitMap.WorldToCell(transform.position), null);
                bgMap.SetTile(pitMap.WorldToCell(transform.position), filledTile);
            }
            Object.Destroy(this.gameObject);
            this.enabled = false;
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
            collidedButton = (collided.tag == "Button");
            collidedPit = (collided.tag == "Pit");
            if (!collidedButton && !collidedPit) return false;
        }

        if (collided && collidedButton) onButton = true;
        if (collided && collidedPit) fellPit = true;
		
        moveDest += dir;
        return true;
    }
}
