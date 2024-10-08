using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyVitals : MonoBehaviour
{
    public bool isEnemyAlive = true;
    public float health = 100;
    public GameObject weaponScript;

    public GameObject eyes;
    // private float lastHitTime = 0f;



    // Start is called before the first frame update
    void Start()
    {
        // lastHitTime = Time.time;

        // health = 100;
        // stamina = 100;
        // mana = 100;
        // sanity = 100;

        // healthPotionCount = 10;
        // manaPotionCount = 10;

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && isEnemyAlive)
        {
            isEnemyAlive = false;
            eyes.SetActive(false);
            GetComponent<Animator>().SetBool("Dead", true);
            weaponScript.GetComponent<dealDamage>().enabled = false;
            GetComponent<Animator>().SetBool("Running", false);
            GetComponent<Animator>().SetBool("Attacking", false);
            GetComponent<Animator>().SetBool("Dead", true);
            GetComponent<EnemyFollow>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;


            health = 0;

            return;
        }
    }

    public void ReduceHealth()
    {
        // if (Time.time - lastHitTime < 1f)
        // {
        //     return;
        // }
        Debug.Log("Reducing Enemy Health");
        health -= 30;
        if (health < 0)
        {
            health = 0;
        }
        Debug.Log("now Enemy Health: " + health);
        // lastHitTime = Time.time;

    }
}

