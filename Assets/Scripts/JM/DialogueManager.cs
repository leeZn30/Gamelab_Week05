using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    public DialogueReader csvReader; // CSV Reader

    GameObject dialogueUI;
    TextMeshProUGUI dialogueText;

    private Queue<string> sentences;
    public bool isDialogueActive = false;
    public bool dialogEnd;

    void Awake()
    {
        dialogueUI = GameObject.Find("DialogueUI");
        dialogueText = dialogueUI.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        sentences = new Queue<string>();
        dialogueUI.SetActive(false);
    }

    void Update()
    {
        // 대화 중이고 E 키를 눌렀을 때 다음 문장을 출력
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void SetDialogueID(int id)
    {
        Dialogue dialogue = GetDialogueById(id);
        if (dialogue != null)
        {
            StartDialogue(dialogue);
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Time.timeScale = 0;
        sentences.Clear();
        dialogueUI.SetActive(true);

        dialogEnd = false;

        foreach (string sentence in dialogue.contexts)
        {
            sentences.Enqueue(sentence);
        }

        isDialogueActive = true; // 첫 번째 대사가 바로 출력되도록 설정
        DisplayNextSentence();   // 첫 번째 대사를 바로 출력
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        Debug.Log(sentence);
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
        dialogueUI.SetActive(false);
        isDialogueActive = false;
        dialogueText.text = "";
        dialogEnd = true;
        Time.timeScale = 1;
    }

    public Dialogue GetDialogueById(int id)
    {
        foreach (Dialogue dialogue in csvReader.GetDialogues())
        {
            if (dialogue.id == id)
                return dialogue;
        }
        return null; // 해당 id를 가진 대사가 없으면 null 반환
    }
}
