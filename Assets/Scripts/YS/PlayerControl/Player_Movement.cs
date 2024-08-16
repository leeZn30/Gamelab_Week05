using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class Player_Movement : MonoBehaviour
{

    //---------------------------------------------------------------------------------------------------------------------------------------------------------

    // Player Movement
    private float horizontalInput;
    private float verticalInput;

    public bool isUsingMap;


    //---------------------------------------------------------------------------------------------------------------------------------------------------------

    void Start()
    {
        isUsingMap = false;

        BattleManager.Instance.Resistance.Add(gameObject);
        //PlayerGunSetting();
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------


    void Update()
    {
        if (!isUsingMap)
        {

            DataManager.Instance.Speed = 4f;

            // Player Movement
            Dock();
            Move();
            Block();

            transform.Translate(Vector2.right * horizontalInput * Time.deltaTime * DataManager.Instance.Speed);
            transform.Translate(Vector2.up * verticalInput * Time.deltaTime * DataManager.Instance.Speed);


            if (InputManager.Instance.controls.Player.Dash.WasPressedThisFrame())
            {
                transform.Translate(Vector2.right * horizontalInput * DataManager.Instance.Speed);
                transform.Translate(Vector2.up * verticalInput * DataManager.Instance.Speed);
            }


            // Check Player Life
            PlayerDeath();

        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------


    void Move()
    {
        Vector2 inputVector = InputManager.Instance.GetMoveValue();
        horizontalInput = inputVector.x;
        verticalInput = inputVector.y;

    }
    void Dock()
    {
        if (InputManager.Instance.controls.Player.SlowMove.WasPressedThisFrame())
        {
            DataManager.Instance.Speed = DataManager.Instance.Speed * 0.5f;
        }
        if (InputManager.Instance.controls.Player.SlowMove.WasReleasedThisFrame())
        {
            DataManager.Instance.Speed = DataManager.Instance.Speed * 2;
        }

    }
    private void Block()
    {
        RaycastHit2D[] hitdown = Physics2D.RaycastAll(transform.position, Vector2.down);

        for (int i = 0; i < hitdown.Length; i++)
        {
            if (hitdown[i].transform != null)
            {
                if (hitdown[i].distance < 0.2f && hitdown[i].collider.CompareTag("Wall"))
                {
                    if (verticalInput < 0)
                    {
                        verticalInput = 0;
                    }
                }

            }
        }

        RaycastHit2D[] hitup = Physics2D.RaycastAll(transform.position, Vector2.up);
        for (int i = 0; i < hitup.Length; i++)
        {
            if (hitup[i].transform != null)
            {
                if (hitup[i].distance < 0.2f && hitup[i].collider.CompareTag("Wall"))
                {
                    if (verticalInput > 0)
                    {
                        verticalInput = 0;
                    }
                }
            }
        }

        RaycastHit2D[] hitleft = Physics2D.RaycastAll(transform.position, Vector2.left);
        for (int i = 0; i < hitleft.Length; i++)
        {
            if (hitleft[i].transform != null)
            {
                if (hitleft[i].distance < 0.2f && hitleft[i].collider.CompareTag("Wall"))
                {
                    if (horizontalInput < 0)
                    {
                        horizontalInput = 0;
                    }
                }

            }
        }

        RaycastHit2D[] hitright = Physics2D.RaycastAll(transform.position, Vector2.right);
        for (int i = 0; i < hitright.Length; i++)
        {
            if (hitright[i].transform != null)
            {
                if (hitright[i].distance < 0.2f && hitright[i].collider.CompareTag("Wall"))
                {
                    if (horizontalInput > 0)
                    {
                        horizontalInput = 0;
                    }
                }
            }
        }


    }


    //---------------------------------------------------------------------------------------------------------------------------------------------------------

    // player Death
    private void PlayerDeath()
    {
        if (DataManager.Instance.isDead)
        {
            Destroy(gameObject);
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------

}
