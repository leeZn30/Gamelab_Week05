using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public int id; // 대사 ID (고유 식별자)

    [Tooltip("캐릭터 이름")]
    public string name;

    [Tooltip("대사 내용")]
    public List<string> contexts; // 대사 내용은 리스트로 유지

    [Tooltip("이벤트 번호")]
    public int number; // 이벤트 번호 (단일 정수)

    [Tooltip("스킵라인")]
    public int skipnum; // 스킵라인 (단일 정수)
}
