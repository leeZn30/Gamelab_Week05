using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
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
                    number = ParseInt(row[3].Trim()), // 정수로 변환하여 설정
                    skipnum = ParseInt(row[4].Trim()) // 정수로 변환하여 설정
                };
            }
            else if (currentDialogue != null)
            {
                // 같은 ID의 추가 문장인 경우
                currentDialogue.contexts.Add(row[2].Trim());
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
