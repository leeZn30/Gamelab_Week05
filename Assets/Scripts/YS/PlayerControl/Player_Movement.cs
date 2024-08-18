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
    public float speed;

    public bool isDashing = false;
    public float dashTime = 0.2f;
    public float dashSpeedMultiplier = 2.0f;

    private bool isDamaged;

    private AudioSource audioSource;
    public AudioClip walkSound;
    public AudioClip dashSound;


    //---------------------------------------------------------------------------------------------------------------------------------------------------------

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DataManager.Instance.Speed = speed;
        DataManager.Instance.MaxHealth = 5f;
        DataManager.Instance.health = DataManager.Instance.MaxHealth;


        isUsingMap = false;
        isDashing = false;

        BattleManager.Instance.Resistance.Add(gameObject);
        //PlayerGunSetting();
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------


    void Update()
    {
        if (!isUsingMap)
        {
            // Player Movement
            Dock();
            Move();
            Block();

            
            // 일반 이동
            transform.Translate(Vector2.right * horizontalInput * Time.deltaTime * DataManager.Instance.Speed);
            transform.Translate(Vector2.up * verticalInput * Time.deltaTime * DataManager.Instance.Speed);
            //audioSource.PlayOneShot(walkSound);

            if (InputManager.Instance.controls.Player.Dash.WasPressedThisFrame() && !isDashing && !DialogueManager.Instance.isDialogueActive)
            {
                // 대시 코루틴 시작
                StartCoroutine(Dash());
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
    private IEnumerator Dash()
    {
        isDashing = true;  // 대시 상태 시작float startTime = Time.time;

        audioSource.PlayOneShot(dashSound);
        // 대시 중 움직임while (Time.time < startTime + dashTime)
        
        DataManager.Instance.Speed *= dashSpeedMultiplier;
        yield return new WaitForSeconds(dashTime);  // 다음 프레임까지 대기
        

        isDashing = false;  // 대시 상태 종료
        DataManager.Instance.Speed = DataManager.Instance.Speed / 2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            isDamaged = true;
            DataManager.Instance.Health--;

            DataManager.Instance.ResetCoolDownTime = 0;
        }
    }
}
