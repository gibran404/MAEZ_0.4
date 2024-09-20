using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class dealDamage : MonoBehaviour
{
    public string itemOwner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if item owner is Player, deal damage to the enemy's health in it's EnemyVitals script
        // if item owner is Enemy, deal damage to the player's health in it's PlayerItemsandVitals script
    }

    void OnTriggerEnter(Collider other)
    {
        if (itemOwner == "Player")
        {
            if (other.tag == "Enemy")
            {
                // reduce enemy health
            }
        }
        else if (itemOwner == "Enemy")
        {
            if (other.tag == "Player")
            {
                // reduce player health from the PlayerItemsandVitals script of the playermanager gameobject
                if (ThirdPersonController.Blocked)
                {
                    FindObjectOfType<PlayerItemsandVitals>().ReduceHealthSmall();

                }
                else
                {
                    FindObjectOfType<PlayerItemsandVitals>().ReduceHealthBig();
                }
                Debug.Log("Player health reduced");
            }
        }
    }
}
