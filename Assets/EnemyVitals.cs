using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyVitals : MonoBehaviour
{
    public bool isEnemyAlive = true;
    public float health = 100;

    private float initialHealth;
    public GameObject weaponScript;

    public GameObject eyes;

    // audio clip to play when takes damage
    public AudioClip HitSound;

    private float lastHitTime = 0f;

    public GameObject HealthFull;
    public GameObject HealthTwoThirds;
    public GameObject HealthOneThird;



    // Start is called before the first frame update
    void Start()
    {
        lastHitTime = Time.time;

        health = 100 + (GameVariables.DungeonSize.x * 5);
        initialHealth = health;

        // health = 100;
        // stamina = 100;
        // mana = 100;
        // sanity = 100;

        // healthPotionCount = 10;
        // manaPotionCount = 10;
        StartCoroutine(RegenerateHealth());
    }

    // Update is called once per frame
    void Update()
    {
        updateHealthBar();
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

            GetComponent<AudioSource>().Stop();

            // disable the parent of the healthFull gameobject
            HealthFull.transform.parent.gameObject.SetActive(false);

            health = 0;

            return;
        }
    }

    private void updateHealthBar()
    {
        if (health > initialHealth*2/3 && HealthFull.activeSelf == false)
        {
            HealthFull.SetActive(true);
            HealthTwoThirds.SetActive(false);
            HealthOneThird.SetActive(false);
        }
        else if (health > initialHealth/3 && health <= initialHealth*2/3 && HealthTwoThirds.activeSelf == false)
        {
            HealthFull.SetActive(false);
            HealthTwoThirds.SetActive(true);
            HealthOneThird.SetActive(false);
        }
        else if (health <= initialHealth/3 && HealthOneThird.activeSelf == false)
        {
            HealthFull.SetActive(false);
            HealthTwoThirds.SetActive(false);
            HealthOneThird.SetActive(true);
        }
    }

    public void ReduceHealth()
    {
        if (Time.time - lastHitTime < 1f)
        {
            return;
        }
        // play hit sound
        AudioSource.PlayClipAtPoint(HitSound, transform.position);

        Debug.Log("Reducing Enemy Health");
        health -= 30;
        if (health < 0)
        {
            health = 0;
        }
        Debug.Log("now Enemy Health: " + health);
        lastHitTime = Time.time;

    }

    private IEnumerator RegenerateHealth()
    {
        while (isEnemyAlive)
        {
            yield return new WaitForSeconds(10f);
            if (health > 0 && health < initialHealth-10)
            {
                health += 10;
                if (health > 100)
                {
                    health = 100;
                }
                Debug.Log("Regenerating Health: " + health);
            }
        }
    }
}

