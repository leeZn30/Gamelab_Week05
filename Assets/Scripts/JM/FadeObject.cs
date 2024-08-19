using UnityEngine;

public class FadeObject : MonoBehaviour
{
    private float fadeDistance = 2.0f; // 플레이어와 물체 사이의 거리
    private float minAlpha = 0.2f; // 최소 투명도 (0은 완전히 투명, 1은 불투명)
    private float fadeSpeed = 5.0f; // 투명해지는 속도

    private Renderer objectRenderer;
    private Color originalColor;
    private Transform playerTransform;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(playerTransform.position, transform.position);
        if (distance < fadeDistance)
        {
            // 플레이어가 가까워질수록 투명해짐
            float alpha = Mathf.Lerp(minAlpha, originalColor.a, distance / fadeDistance);
            SetObjectAlpha(alpha);
        }
        else
        {
            // 플레이어가 멀어지면 원래 색으로 복귀
            SetObjectAlpha(originalColor.a);
        }
    }

    void SetObjectAlpha(float alpha)
    {
        Color newColor = originalColor;
        newColor.a = alpha;
        objectRenderer.material.color = Color.Lerp(objectRenderer.material.color, newColor, fadeSpeed * Time.deltaTime);
    }
}
