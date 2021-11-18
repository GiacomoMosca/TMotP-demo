using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private float textSpeed;
    [SerializeField]
    private List<Dialogue> dialogue;
    [SerializeField]
    private GameObject dialogueBox;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text dialogueText;
    [SerializeField]
    private Image speakerSprite;
    [SerializeField]
    private Image arrowSprite;

    [SerializeField]
    private List<string> names;
    [SerializeField]
    private List<Sprite> sprites;

    private Queue<Dialogue> sentences;
    private Dialogue current;

    private bool isRunning = false;
    private bool isTyping = false;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<Dialogue>(dialogue);
        dialogueBox.SetActive(false);
        nameText.text = "";
        dialogueText.text = "";
        speakerSprite.sprite = null;
        arrowSprite.enabled = false;
    }

    void Update()
    {
        if (isRunning)
        {
            if (Input.GetKeyDown("space"))
            {
                if (isTyping) FinishSentence(current);
                else DisplayNextSentence();
            }
        }
    }

    public void StartDialogue()
    {
        isRunning = true;
        dialogueBox.SetActive(true);
        nameText.text = "";
        dialogueText.text = "";
        speakerSprite.sprite = null;
        arrowSprite.enabled = false;
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (!isRunning) return;
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        current = sentences.Dequeue();
        isTyping = true;
        nameText.text = current.name;
        dialogueText.text = "";
        int index = names.IndexOf(current.name);
        if (index < 0) speakerSprite.sprite = null;
        else speakerSprite.sprite = sprites[index];
        arrowSprite.enabled = false;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(current));
    }

    IEnumerator TypeSentence(Dialogue sentence)
    {
        yield return new WaitForSeconds(0.3f);
        foreach (char letter in sentence.text.ToCharArray())
        {
            dialogueText.text += letter;
            SFXController.instance.PlaySound("dialogue");
            yield return new WaitForSeconds(textSpeed);
        }
        arrowSprite.enabled = true;
        isTyping = false;
    }

    private void FinishSentence(Dialogue sentence)
    {
        StopAllCoroutines();
        dialogueText.text = current.text;
        arrowSprite.enabled = true;
        isTyping = false;
    }

    private void EndDialogue()
    {
        isRunning = false;
        dialogueBox.SetActive(false);
        LevelLoader.instance.UnfreezePlayer();
    }
}
