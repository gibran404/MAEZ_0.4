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
            if (other.tag == "Enemy" && Mouse.current.leftButton.isPressed)
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
                    }
                }
                else if (ThirdPersonController.Blocked)
                {
                    FindObjectOfType<PlayerItemsandVitals>().ReduceHealthSmall();
                }
                else
                {
                    FindObjectOfType<PlayerItemsandVitals>().ReduceHealthBig();
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
            }
            else if (other.tag == "Enemy")
            {
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
    }
}
