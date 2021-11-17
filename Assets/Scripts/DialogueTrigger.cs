using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
   public Dialogue dialogue;

   public void TriggerDialogue()
   {
        DialogueManager manager = FindObjectOfType<DialogueManager>();
        if (!manager) return;
        manager.StartDialogue(dialogue);
        Debug.Log("Trigger");
   }
}
