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
        Debug.Log("디바이스 : " + input.currentControlScheme);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("씬이 로드되었습니다: " + scene.name);

        StartCoroutine(FindPlayerInput());
    }

    private IEnumerator FindPlayerInput()
    {
        yield return new WaitForSeconds(0.1f);  // 씬이 완전히 로드될 때까지 잠시 대기

        playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            Debug.Log("PlayerInput 컴포넌트를 찾았습니다: " + playerInput.name);
            playerInput.onControlsChanged += SwitchControls;
        }
        else
        {
            Debug.LogError("PlayerInput 컴포넌트를 찾을 수 없습니다. 씬에 PlayerInput 컴포넌트가 있는지 확인하세요.");
        }
    }
}
