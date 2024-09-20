using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBehaviour : MonoBehaviour
{
    public float rotateSpeed = 50f;
    public string itemName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // rotate the item horizontally
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    //when player enters the trigger, destroy the item and increase player's state based on the itemName
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (itemName == "HealthPotion")
            {
                PlayerItemsandVitals.healthPotionCount++;
            }
            else if (itemName == "ManaPotion")
            {
                PlayerItemsandVitals.manaPotionCount++;
            }
            Destroy(gameObject);
        }
    }

}   
