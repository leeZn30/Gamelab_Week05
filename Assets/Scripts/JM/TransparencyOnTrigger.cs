using UnityEngine;
using System.Collections;

public class TransparencyOnTrigger : MonoBehaviour
{
    private float transparentAlpha = 0f; // 오브젝트가 투명해질 때의 알파 값
    public float fadeDuration = 0.3f; // 투명하게 되는 데 걸리는 시간
    private SpriteRenderer[] spriteRenderers; // 이 오브젝트와 자식 오브젝트들의 스프라이트 렌더러

    private void Start()
    {
        // 이 오브젝트와 모든 자식 오브젝트들의 SpriteRenderer 컴포넌트를 가져옵니다.
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 투명하게 만드는 코루틴을 시작합니다.
            StartCoroutine(FadeToAlpha(transparentAlpha));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 원래 상태로 복구하는 코루틴을 시작합니다.
            StartCoroutine(FadeToAlpha(1.0f)); // 원래 알파 값은 1입니다.
        }
    }

    // 알파 값을 서서히 변경하는 코루틴
    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        float startAlpha = spriteRenderers[0].color.a; // 현재 알파 값 (첫 번째 스프라이트의 알파 값으로 사용)
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);

            // 모든 스프라이트 렌더러에 대해 알파 값을 변경합니다.
            foreach (var spriteRenderer in spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = newAlpha;
                spriteRenderer.color = color;
            }

            yield return null;
        }

        // 마지막으로 정확한 타겟 알파 값으로 설정합니다.
        foreach (var spriteRenderer in spriteRenderers)
        {
            Color color = spriteRenderer.color;
            color.a = targetAlpha;
            spriteRenderer.color = color;
        }
    }
}
