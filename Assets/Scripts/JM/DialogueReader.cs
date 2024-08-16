using System.Collections.Generic;
using UnityEngine;

public class DialogueReader : MonoBehaviour
{
    public TextAsset csvFile;
    public List<Dialogue> dialogues = new List<Dialogue>();

    void Start()
    {
        ReadCSV();
    }

    void ReadCSV()
    {
        string[] data = csvFile.text.Split(new char[] { '\n' });
        Dialogue currentDialogue = null;

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(',');

            // 줄이 비어있거나 필요한 데이터를 포함하지 않는 경우 스킵
            if (row.Length < 3 || string.IsNullOrWhiteSpace(row[0]))
            {
                if (currentDialogue != null && row.Length > 2)
                {
                    // 같은 ID의 추가 문장인 경우
                    currentDialogue.contexts.Add(row[2].Trim());
                }
                continue;
            }

            // ID가 비어 있지 않다면 새로운 대사 시작
            if (!string.IsNullOrWhiteSpace(row[0]))
            {
                if (currentDialogue != null)
                {
                    dialogues.Add(currentDialogue);
                }

                currentDialogue = new Dialogue
                {
                    id = int.Parse(row[0].Trim()),
                    name = row[1].Trim(),
                    contexts = new List<string> { row[2].Trim() },
                    number = row.Length > 3 && !string.IsNullOrWhiteSpace(row[3]) ? int.Parse(row[3].Trim()) : 0,
                    skipnum = row.Length > 4 && !string.IsNullOrWhiteSpace(row[4]) ? int.Parse(row[4].Trim()) : 0
                };
            }
            else if (currentDialogue != null)
            {
                // 같은 ID의 추가 문장인 경우
                if (row.Length > 2 && !string.IsNullOrWhiteSpace(row[2]))
                {
                    currentDialogue.contexts.Add(row[2].Trim());
                }
            }
        }

        if (currentDialogue != null)
        {
            dialogues.Add(currentDialogue); // 마지막 대사 추가
        }

        foreach (var dialogue in dialogues)
        {
            Debug.Log($"Loaded Dialogue ID: {dialogue.id}, Name: {dialogue.name}, Sentences: {dialogue.contexts.Count}");
        }
    }

    int ParseInt(string value)
    {
        if (int.TryParse(value, out int result))
        {
            return result;
        }
        else
        {
            return 0; // 파싱 실패 시 기본값 0 사용
        }
    }

    public Dialogue[] GetDialogues()
    {
        return dialogues.ToArray();
    }
}
