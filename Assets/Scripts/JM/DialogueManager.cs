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
    private string currentSpeaker; // 현재 대화를 하는 캐릭터의 이름
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

    void DeleteAllBullets()
    {
        // "bullet"이 포함된 모든 오브젝트를 찾아서 삭제
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.ToLower().Contains("bullet"))
            {
                Destroy(obj);
            }
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
        currentSpeaker = dialogue.name; // 대화를 시작할 때 캐릭터 이름을 설정
        foreach (string sentence in dialogue.contexts)
        {
            // 문장이 빈 문자열이거나 공백만 포함된 경우 건너뜁니다.
            if (!string.IsNullOrWhiteSpace(sentence))
            {
                sentences.Enqueue(sentence);
            }
        }
        // 하이라키 창에서 이름에 "bullet"이 포함된 모든 오브젝트를 삭제
        DeleteAllBullets();
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

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = $"{currentSpeaker}: "; // 캐릭터 이름을 먼저 출력foreach (char letter in sentence.ToCharArray())
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
