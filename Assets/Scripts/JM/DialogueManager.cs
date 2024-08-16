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
    public GameObject interactionUI; // E 키 UI (플레이어의 자식 오브젝트로 설정된 UI)
    public int dialogueId; // 대사 ID, 이 NPC가 말할 대사를 지정

    private Queue<string> sentences;
    public bool isDialogueActive = false;
    private bool isPlayerInRange = false; // 플레이어가 NPC 근처에 있는지 확인

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this; // Singleton instance 설정
        DontDestroyOnLoad(this.gameObject); // 이 오브젝트가 씬 전환 시 파괴되지 않도록 설정
    }

    void Start()
    {
        sentences = new Queue<string>();
        dialogueScreen.gameObject.SetActive(false); // 초기에는 대화 UI를 비활성화
        dialogueText.gameObject.SetActive(false);
        interactionUI.SetActive(false); // 처음에는 E 키 UI를 숨깁니다.
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // 대화 진행 중인지 확인
            if (isDialogueActive)
            {
                // 다음 대사로 넘기기
                DisplayNextSentence();
            }
            else
            {
                // 대화 시작
                Dialogue dialogue = GetDialogueById(dialogueId);
                if (dialogue != null)
                {
                    StartDialogue(dialogue);
                }
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
        dialogueText.gameObject.SetActive(false);
        isDialogueActive = false;
        Time.timeScale = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionUI.SetActive(true); // 플레이어가 범위 내에 들어오면 E 키 UI 표시
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionUI.SetActive(false); // 플레이어가 범위 밖으로 나가면 E 키 UI 숨김
        }
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

    public Dialogue[] GetDialogues(int someParameter, int lineY)
    {
        return csvReader.GetDialogues();
    }
}
