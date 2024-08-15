using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon_Rotation : MonoBehaviour
{
    public float rotationSpeed = 720f; // ȸ�� �ӵ� (degrees per second)
    public float angle;
    public Vector2 aimPos;
    public Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        // ���� ��Ʈ�� ��Ŵ Ȯ�� (GamePad �Ǵ� ��Ÿ)
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

        // ������ ���
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ȸ���� Time.deltaTime�� ���� �ε巴�� ����
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon_Rotation : MonoBehaviour
{
    public float horizontalInput;
    public float angle;
    public Vector2 aimPos;
    public Vector2 direction;

    public Vector3 mouseWorldPosition;

    // Update is called once per frame
    void Update()
    {

        if (InputManager.Instance.playerInput.currentControlScheme == "GamePad")
        {
            aimPos = InputManager.Instance.GetAimValue(); 

            if(aimPos.magnitude > 0.1f)
            {
                direction = aimPos;
            }

        }
        else
        {
            aimPos = InputManager.Instance.GetAimValue();
            direction = Camera.main.ScreenToWorldPoint(new Vector3(aimPos.x, aimPos.y, Camera.main.nearClipPlane)) - transform.position;
        }


        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

    }
}
*/