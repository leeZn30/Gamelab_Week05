using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

//------------------------------------------------------------------------------------------------------------------------------------------------------------

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [HideInInspector] public NewInputSystemPlayerControl controls;
    [HideInInspector] public PlayerInput playerInput;
    private Vector2 lastMousePosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        controls = new NewInputSystemPlayerControl();
        playerInput = GetComponent<PlayerInput>();
    }


    //---------------------------------------------------------------------------------------------------------------------------------------------------------

    private void OnEnable()
    {
        controls.Enable();
        if (playerInput != null)
        {
            playerInput.onControlsChanged += SwitchControls;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.onControlsChanged -= SwitchControls;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (controls != null)
        {
            controls.Disable();
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------

    public Vector2 GetAimValue()
    {
        return controls.Player.Aim.ReadValue<Vector2>();
    }

    public Vector2 GetMoveValue()
    {
        return controls.Player.Move.ReadValue<Vector2>();
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------

    private void SwitchControls(PlayerInput input)
    {
        Debug.Log("����̽� : " + input.currentControlScheme);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("���� �ε�Ǿ����ϴ�: " + scene.name);

        StartCoroutine(FindPlayerInput());
    }

    private IEnumerator FindPlayerInput()
    {
        yield return new WaitForSeconds(0.1f);  // ���� ������ �ε�� ������ ��� ���

        playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            Debug.Log("PlayerInput ������Ʈ�� ã�ҽ��ϴ�: " + playerInput.name);
            playerInput.onControlsChanged += SwitchControls;
        }
        else
        {
            Debug.LogError("PlayerInput ������Ʈ�� ã�� �� �����ϴ�. ���� PlayerInput ������Ʈ�� �ִ��� Ȯ���ϼ���.");
        }
    }
}
