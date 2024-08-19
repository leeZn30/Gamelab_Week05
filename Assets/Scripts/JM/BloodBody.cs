using UnityEngine;

public class BloodBody : MonoBehaviour
{
    public GameObject bloodEffectPrefab; // 플레이어 발 아래에 생성될 이미지 프리팹
    public AudioClip bloodSound; // 발생할 소리
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // 만약 AudioSource가 없다면 자동으로 추가합니다.
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어 발 아래 위치
            Vector3 bloodPosition = other.transform.position;
            bloodPosition.z = 0; // Z축 위치를 0으로 고정하여 UI에 나타나도록 설정

            // 피 효과 이미지 생성
            Instantiate(bloodEffectPrefab, bloodPosition, Quaternion.identity);

            // 피 소리 재생
            audioSource.PlayOneShot(bloodSound);
        }
    }
}
