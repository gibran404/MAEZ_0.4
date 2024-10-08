using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public Animator animator;

    public bool Aggro = false;
    public bool canAttack = false;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyVitals>().isEnemyAlive == false)
        {
            return;
        }   

        if (Aggro)
        {
            enemy.SetDestination(player.position);
            transform.LookAt(player);
        }
        else
        {
            if (animator.GetBool("Running") == true || animator.GetBool("Attacking") == true)
            {
                animator.SetBool("Attacking", false);
                animator.SetBool("Running", false);
            }
            return;
        }
        
        if (animator.GetBool("Running") == false && enemy.velocity.magnitude > 1f) 
        {
            animator.SetBool("Running", true);
        }
        else if (canAttack)
        {
            animator.SetBool("Attacking", true);
        }
        else
        {
            animator.SetBool("Attacking", false);
            animator.SetBool("Running", false);
        }


        // if (animator.GetBool("Running") == true && enemy.velocity.magnitude < 1f)
        // {
        //     animator.SetBool("Running", false);
        // }

        // if (animator.GetBool("Running") == false && Attacking == false)
        // {
        //     animator.SetBool("Idle", true);
        // }
        // else
        // {
        //     animator.SetBool("Idle", false);
        // }

        // make enemy look at player
    }
    
}
