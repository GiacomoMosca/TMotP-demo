using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonControler : MonoBehaviour
{
    private List<string> interactableObjects;
    [Header("Push Button")]
    [Header("Leave Button")]
    public UnityEvent PushButton;
    public UnityEvent LeaveButton;
    // Start is called before the first frame update
    void Start()
    {
        interactableObjects = new List<string> { "Pushable" };
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (interactableObjects.Contains(LayerMask.LayerToName(other.gameObject.layer)))
        {
            PushButton.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (interactableObjects.Contains(LayerMask.LayerToName(other.gameObject.layer)))
        {
            LeaveButton.Invoke();
        }
    }
}
