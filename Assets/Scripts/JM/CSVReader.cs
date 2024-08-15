using UnityEngine;
using System.Collections.Generic;

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

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(',');

            Dialogue dialogue = new Dialogue
            {
                id = int.Parse(row[0]), // id를 정수로 파싱하여 저장
                name = row[1],
                contexts = new string[] { row[2] },
                number = new string[] { row[3] },
                skipnum = new string[] { row[4] }
            };

            dialogues.Add(dialogue);
        }
    }

    public Dialogue[] GetDialogues()
    {
        return dialogues.ToArray();
    }
}
