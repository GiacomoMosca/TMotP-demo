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

    public Sprite baseSprite;
    public Sprite playerSprite;
    private SpriteRenderer spriteRenderer;

    private bool moveReady = true;
    private float moveTimer = 0f;
    private bool shouldBreak = false;

    private Tilemap sandMap;

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
        if (Vector3.Distance(transform.position, playerController.moveDest) == 0f && shouldBreak)
        {
            DestroyForm();
        }

        player.transform.position = Vector3.MoveTowards(player.transform.position, playerController.moveDest, playerController.moveSpeed * Time.deltaTime * 1.5f);

        if (Vector3.Distance(transform.position, playerController.moveDest) == 0f)
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
        shouldBreak = true;
        for (int i = 0; i < 100 && !Physics2D.OverlapCircle(playerController.moveDest + dir, .2f); i++)
        {
            playerController.moveDest += dir;
            if (sandMap.HasTile(sandMap.WorldToCell(playerController.moveDest)))
            {
                shouldBreak = false;
                break;
            }
        }
        moveTimer = playerController.moveDelay;
        moveReady = false;
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
        Object.Destroy(this.gameObject);
        playerController.FormDestroyed();
    }

}