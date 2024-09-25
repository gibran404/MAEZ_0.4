using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{
    private bool playerInRange;
    public GameObject E_Label;

    // Update is called once per frame
    void Update()
    {
        //when player is in range and presses E, destroy the chest and spawn the items
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(E_Label);

            GetComponent<Animator>().SetBool("Open", true);
            GetComponent<ItemSpawner>().spawnItems();
            
            Destroy(this);
        }

        
    }

    //when player in collider range, enable the Elabel and switch the bool.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = true;
            E_Label.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
            E_Label.SetActive(false);
        }
    }
}
