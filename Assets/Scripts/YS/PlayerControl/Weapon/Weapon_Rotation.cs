using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon_Rotation : MonoBehaviour
{
    public float rotationSpeed = 720f; // 회전 속도 (degrees per second)
    public float angle;
    public Vector2 aimPos;
    public Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        // 현재 컨트롤 스킴 확인 (GamePad 또는 기타)
        aimPos = InputManager.Instance.GetAimValue();

        if (InputManager.Instance.playerInput.currentControlScheme == "GamePad")
        {
            if (aimPos.magnitude > 0.1f)
            {
                direction = aimPos;
            }
        }
        else
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(aimPos.x, aimPos.y, Camera.main.nearClipPlane));
            direction = mouseWorldPosition - transform.position;
        }

        // 각도를 
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 회전을 Time.deltaTime에 따라 부드럽게 적용
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
