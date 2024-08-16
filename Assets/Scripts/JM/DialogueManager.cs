using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    public DialogueReader csvReader; // CSV Reader

    public TextMeshProUGUI dialogueText;
    public Image dialogueScreen;

    private Queue<string> sentences;
    public bool isDialogueActive = false;
    public bool dialogEnd;

    private NPCInteraction currentNPCInteraction;
    private QuestNPCInteraction currentQuestNPCInteraction;

    void Start()
    {
        sentences = new Queue<string>();
        dialogueScreen.gameObject.SetActive(false); // 초기에는 대화 UI를 비활성화
        dialogueText.gameObject.SetActive(false);
    }

    void Update()
    {
        // 대화 중이고 E 키를 눌렀을 때 다음 문장을 출력
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void RegisterNPCInteraction(NPCInteraction npcInteraction)
    {
        currentNPCInteraction = npcInteraction;
        currentQuestNPCInteraction = null; // 일반 NPC와 상호작용이 등록되면 퀘스트 NPC 상호작용 해제
    }

    public void RegisterQuestNPCInteraction(QuestNPCInteraction questNPC)
    {
        currentQuestNPCInteraction = questNPC;
        currentNPCInteraction = null; // 퀘스트 NPC와 상호작용이 등록되면 일반 NPC 상호작용 해제
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
        dialogueScreen.gameObject.SetActive(true); // 대화 시작 시 UI 활성화
        dialogueText.gameObject.SetActive(true);

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
        dialogueScreen.gameObject.SetActive(false); // 대화 종료 시 UI 비활성화
        isDialogueActive = false;
        dialogueText.text = "";
        dialogEnd = true;
        Time.timeScale = 1;

        // 대화가 끝나면 현재 NPC 상호작용 상태를 해제
        currentNPCInteraction = null;
        currentQuestNPCInteraction = null;
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
