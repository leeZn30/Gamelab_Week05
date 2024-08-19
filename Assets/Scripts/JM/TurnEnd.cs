using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class TurnEnd : Singleton<TurnEnd>
{
    public Image uiImage; // 투명해질 UI 이미지
    public float fadeDuration = 2f; // 투명해지고 다시 불투명해지는 데 걸리는 시간
    public float waitTime = 1f; // 투명해진 후 대기 시간
    public int fadeCycles = 1; // 투명/불투명 사이클 횟수

    private bool isFading = false;


    public IEnumerator FadeInAndOut()
    {
        isFading = true;
        Color originalColor = uiImage.color;
        float time = 0;

        for (int i = 0; i < fadeCycles; i++)
        {
            // Fade in (불투명해짐)
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
                uiImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            time = 0;
        }



    }
}
