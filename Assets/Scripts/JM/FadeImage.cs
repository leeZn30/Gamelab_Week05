using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class FadeImage : MonoBehaviour
{
    public Image uiImage; // 투명해질 UI 이미지
    public float fadeDuration = 2f; // 투명해지고 다시 불투명해지는 데 걸리는 시간
    public float waitTime = 1f; // 투명해진 후 대기 시간
    public int fadeCycles = 1; // 투명/불투명 사이클 횟수
    public int dialogueId; // 출력할 기본 대사 ID
    public TextMeshProUGUI endText; // "The End"를 나타낼 텍스트

    private bool isFading = false;

    void Start()
    {
        if (uiImage == null)
        {
            uiImage = GetComponent<Image>();
        }

        if (uiImage != null)
        {
            StartCoroutine(FadeInAndOut());
        }
    }

    private IEnumerator FadeInAndOut()
    {
        isFading = true;
        Color originalColor = uiImage.color;
        float time = 0;

        for (int i = 0; i < fadeCycles; i++)
        {
            // Fade out (투명해짐)
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
                uiImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            time = 0;
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.SetDialogueID(dialogueId);
            }
            // Wait for some time after fading out
            yield return new WaitForSeconds(waitTime);

            // Fade in (불투명해짐)
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
                uiImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            time = 0;

            // Wait for some time after fading in (optional)
            yield return new WaitForSeconds(waitTime);
        }
        if (endText != null)
        {
            endText.gameObject.SetActive(true);
        }
        // 최종적으로 원래 상태로 복구
        uiImage.color = originalColor;
        isFading = false;

        // "The End" 텍스트 표시

    }
}
