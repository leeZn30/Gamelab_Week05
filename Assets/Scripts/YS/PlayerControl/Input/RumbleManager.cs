using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.DualShock;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager instance;

    private Gamepad pad;

    private Coroutine stopRumbleAfterCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void RumblePulse(float lowFrequency, float highFrequency, float duration)
    {
        if (InputManager.Instance.playerInput.currentControlScheme == "GamePad")
        {
            // 현재 게임패드를 가져옵니다.
            pad = Gamepad.current;

            // 게임패드가 연결되어 있는지 확인합니다.
            if (pad != null)
            {
                // 게임패드 타입에 따라 다른 진동 설정을 합니다.
                if (pad is XInputController)
                {
                    // Xbox 패드 진동 설정 (예: 약한 진동)
                    lowFrequency *= 0.8f;
                    highFrequency *= 0.8f;
                }
                else if (pad is DualShockGamepad)
                {
                    // DualShock 패드 진동 설정 (예: 강한 진동)
                    lowFrequency *= 1f;
                    highFrequency *= 1f;
                }

                // 진동 시작
                pad.SetMotorSpeeds(lowFrequency, highFrequency);

                // 일정 시간 후에 진동을 멈춥니다.
                if (stopRumbleAfterCoroutine != null)
                {
                    StopCoroutine(stopRumbleAfterCoroutine);
                }
                stopRumbleAfterCoroutine = StartCoroutine(StopRumble(duration, pad));
            }
        }
    }

    private IEnumerator StopRumble(float duration, Gamepad pad)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        pad.SetMotorSpeeds(0f, 0f);
    }

    private void OnDisable()
    {
        // 모든 게임패드의 진동을 멈춥니다.
        var gamepads = Gamepad.all;
        foreach (var gamepad in gamepads)
        {
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }
}
