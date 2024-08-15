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
    public bool isDialogueActive = false;
    public bool dialogEnd;

    void Start()
    {
        sentences = new Queue<string>();
        dialogueScreen.gameObject.SetActive(false); // 초기에는 대화 UI를 비활성화
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Time.timeScale = 0;

        isDialogueActive = true;
        sentences.Clear();
        dialogueScreen.gameObject.SetActive(true); // 대화 시작 시 UI 활성화
        dialogEnd = false;

        foreach (string sentence in dialogue.contexts)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
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
        dialogueScreen.gameObject.SetActive(false); // 대화 종료 시 UI 비활성화
        isDialogueActive = false;
        dialogueText.text = "";
        dialogEnd = true;
        Time.timeScale = 1;

    }
}
