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
            // ���� �����е带 �����ɴϴ�.
            pad = Gamepad.current;

            // �����е尡 ����Ǿ� �ִ��� Ȯ���մϴ�.
            if (pad != null)
            {
                // �����е� Ÿ�Կ� ���� �ٸ� ���� ������ �մϴ�.
                if (pad is XInputController)
                {
                    // Xbox �е� ���� ���� (��: ���� ����)
                    lowFrequency *= 0.8f;
                    highFrequency *= 0.8f;
                }
                else if (pad is DualShockGamepad)
                {
                    // DualShock �е� ���� ���� (��: ���� ����)
                    lowFrequency *= 1f;
                    highFrequency *= 1f;
                }

                // ���� ����
                pad.SetMotorSpeeds(lowFrequency, highFrequency);

                // ���� �ð� �Ŀ� ������ ����ϴ�.
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
        // ��� �����е��� ������ ����ϴ�.
        var gamepads = Gamepad.all;
        foreach (var gamepad in gamepads)
        {
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }
}
