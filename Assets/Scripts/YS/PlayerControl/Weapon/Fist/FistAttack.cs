using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FistAttack : MonoBehaviour
{
    private Animator animator;

    public bool hand;
    public bool isAttacking;

    public float punchDelay;
    public float delayTime;

    // Start is called before the first frame update
    void OnEnable()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(PunchDelay());

        delayTime = 0;
        punchDelay = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.controls.Player.Shoot.WasPressedThisFrame())
        {
            DataManager.Instance.ResetCoolDownTime = 0;

            if (delayTime > punchDelay)
            {
                isAttacking = true;
                animator.SetBool("IsAttacking", true);

                StartCoroutine(Delay());
            }
        }


    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1f);
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    IEnumerator PunchDelay()
    {
        while (true)
        {
            yield return null;
            delayTime += Time.time;

        }
    }
}
