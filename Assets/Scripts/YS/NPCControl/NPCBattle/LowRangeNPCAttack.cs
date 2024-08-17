using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowRangeNPCAttack : MonoBehaviour
{

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        GetComponent<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.parent.parent.parent.GetComponent<NPCInfo>().target != null){
            if (Vector2.Distance(transform.parent.parent.parent.GetComponent<NPCInfo>().target.transform.position, transform.parent.parent.parent.position) < 2)
            {
                GetComponent<BoxCollider2D>().enabled = true;
                animator.SetBool("IsAttack", true);
            }
            else
            {
                GetComponent<BoxCollider2D>().enabled = false;
                animator.SetBool("IsAttack", false);
            }

        }
    }
}
