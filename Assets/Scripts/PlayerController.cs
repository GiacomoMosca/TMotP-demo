using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float moveDelay;
    public Transform moveDest;
    public LayerMask stopsMove;

    private bool moveReady = true;
    private float moveTimer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        moveDest.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveDest.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, moveDest.position) == 0f)
        {
            if (moveTimer <= 0f) moveReady = true;
            else moveTimer -= Time.deltaTime;
        }

        if (moveReady)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                MakeMove(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), moveDelay);
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                MakeMove(new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), moveDelay);
            }
        }
    }
    void MakeMove(Vector3 dir, float delay)
    {
        if (!Physics2D.OverlapCircle(moveDest.position + dir, .2f, stopsMove))
        {
            moveDest.position += dir;
            moveReady = false;
            moveTimer = delay;
        }
    }
}
