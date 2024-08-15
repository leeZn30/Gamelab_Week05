using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public Image dialogueScreen;

    private Queue<string> sentences;
    private bool isDialogueActive = false;
    public bool dialogEnd;

    void Start()
    {
        sentences = new Queue<string>();
    }

    void Update()
    {
        if (isDialogueActive && InputManager.Instance.controls.Player.Interaction.WasPerformedThisFrame())
        {
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        sentences.Clear();
        dialogueScreen.gameObject.SetActive(true);
        dialogEnd = false;

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        dialogueScreen.gameObject.SetActive(false);
        isDialogueActive = false;
        dialogueText.text = "";
        dialogEnd = true;
    }
    public bool Dialog
    {
        get { return dialogEnd; }
        set { dialogEnd = value; }
    }
}
