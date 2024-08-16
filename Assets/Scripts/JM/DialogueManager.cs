using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    public static DialogueManager instance; // Singleton instance
    public DialogueReader csvReader; // CSV Reader

    public TextMeshProUGUI dialogueText;
    public Image dialogueScreen;

    private Queue<string> sentences;
    public bool isDialogueActive = false;
    public bool dialogEnd;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        sentences = new Queue<string>();
        dialogueScreen.gameObject.SetActive(false); // 초기에는 대화 UI를 비활성화
        dialogueText.gameObject.SetActive(false);
    }

    public void HandleQuestInteraction(QuestNPCInteraction questNPC)
    {
        if (isDialogueActive)
        {
            // 대화 진행 중이면 다음 대사로 넘기기
            DisplayNextSentence();
        }
        else
        {
            Dialogue dialogue = null;

            if (questNPC.questDialogueCompleted)
            {
                // 퀘스트 완료 후 대화가 한 번 완료되었다면, 이후에는 항상 postCompletionDialogueId 출력
                dialogue = GetDialogueById(questNPC.postCompletionDialogueId);
            }
            else if (!questNPC.quest.isAvailable)
            {
                // 퀘스트가 아직 사용 가능하지 않다면, 처음 대사만 출력
                dialogue = GetDialogueById(questNPC.initialDialogueId);
                if (dialogue != null)
                {
                    questNPC.quest.isAvailable = true; // 퀘스트를 사용 가능 상태로 변경
                }
            }
            else if (!questNPC.quest.isCompleted)
            {
                // 퀘스트가 사용 가능하지만 아직 완료되지 않은 상태라면
                dialogue = GetDialogueById(questNPC.incompleteQuestDialogueId);
            }
            else if (questNPC.quest.isCompleted)
            {
                // 퀘스트가 완료된 상태라면
                dialogue = GetDialogueById(questNPC.completedQuestDialogueId);
                if (dialogue != null)
                {
                    questNPC.questDialogueCompleted = true; // 퀘스트 완료 후 대화가 한 번 완료되었음을 기록
                }
            }

            if (dialogue != null)
            {
                StartDialogue(dialogue);
            }
        }
    }

    public void HandleInteraction(NPCInteraction npcInteraction)
    {
        if (isDialogueActive)
        {
            // 대화 진행 중이면 다음 대사로 넘기기
            DisplayNextSentence();
        }
        else
        {
            Dialogue dialogue = GetDialogueById(npcInteraction.dialogueId);
            if (dialogue != null)
            {
                StartDialogue(dialogue);
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Time.timeScale = 0;
        isDialogueActive = true;
        sentences.Clear();
        dialogueScreen.gameObject.SetActive(true); // 대화 시작 시 UI 활성화
        dialogueText.gameObject.SetActive(true);

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
