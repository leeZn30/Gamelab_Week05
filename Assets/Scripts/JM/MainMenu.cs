using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // 게임 시작 버튼을 눌렀을 때 호출됩니다.
    public void StartGame()
    {
        // "GameScene"이라는 이름의 씬을 로드합니다.
        // 씬 이름은 Unity의 빌드 설정에서 설정한 이름으로 바꿔주세요.
        SceneManager.LoadScene("01_GameScene");
    }

    // 나가기 버튼을 눌렀을 때 호출됩니다.
    public void QuitGame()
    {
        // 실제 빌드된 애플리케이션을 종료합니다.
        Application.Quit();
    }
}
