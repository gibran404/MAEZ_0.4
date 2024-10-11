using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public Animator animator;

    public Vector2 initialPosition;

    public bool Aggro = false;
    public bool canAttack = false;

    public bool fleeing = false;

    public bool soundPlaying;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;

        initialPosition = new Vector2(transform.position.x, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        var enemyVitals = GetComponent<EnemyVitals>();

        if (PlayerItemsandVitals.isplayerAlive == false && Aggro)
        {
            //return to initial position
            enemy.SetDestination(new Vector3(initialPosition.x, transform.position.y, initialPosition.y));
            Aggro = false;
            canAttack = false;
            fleeing = true;

            animator.SetBool("Running", true);
            animator.SetBool("Attacking", false);


            return;
        }
        if (enemyVitals.isEnemyAlive == false)
        {
            transform.LookAt(player);
            return;
        }

        if (fleeing)
        {
            enemy.SetDestination(new Vector3(initialPosition.x, transform.position.y, initialPosition.y));
            if (Vector3.Distance(transform.position, new Vector3(initialPosition.x, transform.position.y, initialPosition.y)) < 2f)
            {
                fleeing = false;
            }
            return;
        }
        else if (enemyVitals.health < 20 && !fleeing && Vector3.Distance(transform.position, new Vector3(initialPosition.x, transform.position.y, initialPosition.y)) > 5f)
        {
            Aggro = false;
            canAttack = false;
            fleeing = true;
        }

        if (enemyVitals.health > 60 && fleeing)
        {
            fleeing = false;
        }

        

        if (Aggro)
        {
            if (!soundPlaying)
            {
                GetComponent<AudioSource>().Play();
                soundPlaying = true;
            }
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

            if (soundPlaying)
            {
                soundPlaying = false;
                GetComponent<AudioSource>().Stop();
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
    }
    
}
