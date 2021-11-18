using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VaseController : MonoBehaviour, IForm
{
    public int id;

    private GameObject player;
    private PlayerController playerController;
    private PushableController pushableController;

    [SerializeField]
    private Sprite baseSprite;
    [SerializeField]
    private Sprite playerSprite;
    private SpriteRenderer spriteRenderer;

    private bool moveReady = true;
    private float moveTimer = 0f;
    private bool atDestination = false;
    private bool isRolling = false;
    private bool shouldBreak = false;

    private Tilemap sandMap;

    private bool fellPit = false;

    void Start()
    {
        this.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
        pushableController = this.gameObject.GetComponent<PushableController>();
        playerController = player.GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sandMap = GameObject.FindGameObjectWithTag("Sand").GetComponent<Tilemap>();
    }

    void Update()
    {
        atDestination = Vector3.Distance(transform.position, playerController.moveDest) == 0f;

        if (!atDestination)
        {
            isRolling = true;
            player.transform.position = Vector3.MoveTowards(player.transform.position, playerController.moveDest, playerController.moveSpeed * Time.deltaTime * 1.5f);
        }
        else
        {
            if (isRolling)
            {
                isRolling = false;
                SFXController.instance.StopSound();
            }
            if (fellPit)
            {
                Object.Destroy(this.gameObject);
                playerController.GameOver();
                return;
            }
            else if (shouldBreak)
            {
                DestroyForm();
                return;
            }
            else
            {
                if (moveTimer <= 0f) moveReady = true;
                else moveTimer -= Time.deltaTime;
            }
        }

        if (moveReady && !playerController.isFrozen && !playerController.isPaused)
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
        shouldBreak = true;
        Collider2D collided = null;

        for (int i = 0; i < 100; i++)
        {
            collided = Physics2D.OverlapCircle(playerController.moveDest + dir, .2f);
            if (collided) break;
            playerController.moveDest += dir;
            if (sandMap.HasTile(sandMap.WorldToCell(playerController.moveDest)))
            {
                shouldBreak = false;
                break;
            }
        }

        if (collided && collided.tag == "Pit")
        {
            playerController.moveDest += dir;
            fellPit = true;
        }
        moveTimer = playerController.moveDelay;
        moveReady = false;

        if (Vector3.Distance(transform.position, playerController.moveDest) != 0f) SFXController.instance.PlaySound("vaseRoll");
    }

    public void Wake()
    {
        this.enabled = true;
        pushableController.enabled = false;
        UIManager.instance.setForm(id);
        spriteRenderer.sprite = playerSprite;
        moveReady = false;
        moveTimer = playerController.moveDelay;
        shouldBreak = false;
        UIManager.instance.setStep(-1);
    }

    public void Sleep()
    {
        this.enabled = false;
        pushableController.enabled = true;
        spriteRenderer.sprite = baseSprite;
    }

    void DestroyForm()
    {
        SFXController.instance.PlaySound("destroyVase");
        Object.Destroy(this.gameObject);
        playerController.FormDestroyed();
    }

}