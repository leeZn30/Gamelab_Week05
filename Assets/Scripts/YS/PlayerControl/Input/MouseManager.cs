using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseManager : MonoBehaviour
{
    private void Update()
    {

        if (InputManager.Instance.playerInput.currentControlScheme == "GamePad")
        {
            if (Mouse.current != null && Mouse.current.enabled)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                InputSystem.DisableDevice(Mouse.current);
            }
        }
        else
        {
            if (Mouse.current != null && !Mouse.current.enabled)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                InputSystem.EnableDevice(Mouse.current);
            }
        }
    }

}
