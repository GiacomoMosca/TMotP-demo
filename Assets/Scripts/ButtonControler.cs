using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ButtonControler : MonoBehaviour
{
    [SerializeField]
    private LayerMask interactableObjects;
    [SerializeField]
    private List<GameObject> doors;
    [SerializeField]
    private Sprite closedSprite;
    [SerializeField]
    private Sprite openSprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (interactableObjects == (interactableObjects | (1 << other.gameObject.layer)))
        {
            foreach (GameObject door in doors) {
                door.GetComponent<BoxCollider2D>().enabled = false;
                door.GetComponent<SpriteRenderer>().sprite = openSprite;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (interactableObjects == (interactableObjects | (1 << other.gameObject.layer)))
        {
            foreach (GameObject door in doors)
            {
                door.GetComponent<BoxCollider2D>().enabled = true;
                door.GetComponent<SpriteRenderer>().sprite = closedSprite;
            }
        }
    }
}
