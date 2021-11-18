using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MummyController : MonoBehaviour, IForm
{
    public int id;

    private GameObject player;
    private PlayerController playerController;

    [SerializeField]
    private Sprite baseSprite;
    [SerializeField]
    private Sprite playerSprite;
    private SpriteRenderer spriteRenderer;

    public int maxMove;
    private bool moveReady = true;
    private float moveTimer = 0f;
    private int moveCount = 0;
    private bool atDestination = false;
    private Vector3 startingPos;

    [SerializeField]
    private Tile sandTile;
    private Tilemap sandMap;
    private bool isOnSandTile = false;
    private bool isSandy = false;

    private bool fellPit = false;

    void Start()
    {
        this.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        sandMap = GameObject.FindGameObjectWithTag("Sand").GetComponent<Tilemap>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingPos = transform.position;
        sandMap = GameObject.FindGameObjectWithTag("Sand").GetComponent<Tilemap>();
    }

    void Update()
    {
        atDestination = Vector3.Distance(transform.position, playerController.moveDest) == 0f;

        if (!atDestination)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, playerController.moveDest, playerController.moveSpeed * Time.deltaTime);
        }
        else
        {
            if (fellPit)
            {
                Object.Destroy(this.gameObject);
                playerController.GameOver();
                return;
            }

            if (moveTimer <= 0f) moveReady = true;
            else moveTimer -= Time.deltaTime;

            if (!isSandy && isOnSandTile)
            {
                isSandy = true;
            }
            else if (isSandy && !isOnSandTile)
            {
                sandMap.SetTile(sandMap.WorldToCell(transform.position), sandTile);
                isOnSandTile = true;
            }
        }


        if (moveReady && !playerController.isFrozen && !playerController.isPaused)
        {
            if (Input.GetKeyDown("space"))
            {
                DestroyForm();
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
        Collider2D collided = Physics2D.OverlapCircle(playerController.moveDest + dir, .2f);
        bool collidedPushable = false;
        bool collidedPit = false;

        if (collided)
        {
            collidedPushable = playerController.pushable == (playerController.pushable | (1 << collided.gameObject.layer));
            collidedPit = collided.tag == "Pit";
            if (!collidedPushable && !collidedPit) return;
        }

        if (moveCount >= maxMove)
        {
            DestroyForm();
        }
        else
        {
            if (collided && collidedPit) fellPit = true;
            if (collided && collidedPushable && !collided.gameObject.GetComponent<PushableController>().TryMove(dir)) return;

            playerController.moveDest += dir;
            moveReady = false;
            moveTimer = playerController.moveDelay;
            SetMoveCount(moveCount + 1);
            SFXController.instance.PlaySound("mummyStep");
            isOnSandTile = sandMap.HasTile(sandMap.WorldToCell(playerController.moveDest));
        }
    }

    void SetMoveCount(int count)
    {
        moveCount = count;
        UIManager.instance.setStep(maxMove - count);
    }

    public void Wake()
    {
        this.enabled = true;
        UIManager.instance.setForm(id);
        spriteRenderer.sprite = playerSprite;
        isSandy = false;
        moveReady = false;
        moveTimer = playerController.moveDelay;
        SetMoveCount(0);
    }

    public void Sleep()
    {
        this.enabled = false;
        spriteRenderer.sprite = baseSprite;
        moveCount = 0;
    }

    void DestroyForm()
    {
        SFXController.instance.PlaySound("mummyDeath");
        Object.Destroy(this.gameObject);
        playerController.FormDestroyed();
        player.transform.position = startingPos;
        playerController.moveDest = startingPos;
    }

}
