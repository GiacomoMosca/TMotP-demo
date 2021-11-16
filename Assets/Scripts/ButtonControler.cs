using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonControler : MonoBehaviour
{
    private List<string> availableForms;
    [Header("push button")]
    [Header("leave button")]
    public UnityEvent PushButton;
    public UnityEvent LeaveButton;
    // Start is called before the first frame update
    void Start()
    {
        availableForms = new List<string> { "Box" };
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (availableForms.Contains(other.tag))
        {
            Debug.Log("Push");
            PushButton.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (availableForms.Contains(other.tag))
        {
            Debug.Log("Leave");
            LeaveButton.Invoke();
        }
    }
}
