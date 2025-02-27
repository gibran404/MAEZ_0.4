using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBehaviour : MonoBehaviour
{
    public float rotateSpeed = 50f;
    public string itemName;
    [SerializeField] public float probability = 0f;

    public AudioClip pickupSound;

    // collider
    public Collider triggerCollider;

    // Start is called before the first frame update
    void Start()
    {
        //disable the trigger collider and enable it after 1 second
        triggerCollider.enabled = false;
        StartCoroutine(EnableCollider());

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
            // play the audio source
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            
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

    //enable the trigger collider after 1 second
    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(1);
        triggerCollider.enabled = true;
    }
}   
