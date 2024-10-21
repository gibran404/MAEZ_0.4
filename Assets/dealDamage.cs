using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class dealDamage : MonoBehaviour
{
    public string itemOwner;
    public GameObject ownerobject;
    private float LastDamageTime;

    public AudioClip HitSound;
    public AudioClip BlockSound;

    public string DamageAmount;
    // Start is called before the first frame update
    void Start()
    {
        LastDamageTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    void OnTriggerEnter(Collider other)
    {
        if (itemOwner == "Player")
        {
            // and left mouse button is held down
            if (other.tag == "Enemy" && (Mouse.current.leftButton.isPressed || UIVariables.UIAttacking))
            {
                // if (Time.time - LastDamageTime < 1f)
                // {
                //     return;
                // }

                Debug.Log("Enemy hit");
                // reduce enemy health
                if (other.GetComponent<EnemyVitals>().isEnemyAlive)
                {
                    other.GetComponent<EnemyVitals>().ReduceHealth();
                    Debug.Log("Enemy health reduced");
                    // LastDamageTime = Time.time;
                }
                else
                {
                    Debug.Log("Enemy is already dead");
                }
            }
        }
        else if (itemOwner == "Enemy" && ownerobject.GetComponent<EnemyVitals>().isEnemyAlive) // && ownerobject is attacking
        {
            if (other.tag == "Player")
            {
                if (Time.time - LastDamageTime < 1f)
                {
                    return;
                }
                if (DamageAmount == "Small")
                {
                    if (!ThirdPersonController.Blocked)
                    {
                        FindObjectOfType<PlayerItemsandVitals>().ReduceHealthSmall();
                        AudioSource.PlayClipAtPoint(HitSound, transform.position);
                    }
                    else
                    {
                        AudioSource.PlayClipAtPoint(BlockSound, transform.position);
                    }
                }
                else if (ThirdPersonController.Blocked)
                {
                    FindObjectOfType<PlayerItemsandVitals>().ReduceHealthSmall();
                    AudioSource.PlayClipAtPoint(HitSound, transform.position);
                }
                else
                {
                    FindObjectOfType<PlayerItemsandVitals>().ReduceHealthBig();
                    AudioSource.PlayClipAtPoint(HitSound, transform.position);
                }
                Debug.Log("Player health reduced from "+ itemOwner);
                LastDamageTime = Time.time;
            }
        }
        else if (itemOwner == "Trap")
        {
            if (other.tag == "Player")
            {
                FindObjectOfType<PlayerItemsandVitals>().ReduceHealthSmall();
                FindObjectOfType<PlayerItemsandVitals>().ReduceHealthSmall();
                
                // AudioSource.PlayClipAtPoint(HitSound, transform.position);

            }
            else if (other.tag == "Enemy")
            {
                if (other.GetComponent<EnemyVitals>().isEnemyAlive)
                {
                    other.GetComponent<EnemyVitals>().ReduceHealth();
                    other.GetComponent<EnemyVitals>().ReduceHealth();

                    Debug.Log("Enemy health reduced");
                    // LastDamageTime = Time.time;
                }
                else
                {
                    Debug.Log("Enemy is already dead");
                }
            }
        }
    }
}
