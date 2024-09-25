using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    //array of gameobject to be spawned
    public GameObject[] items;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnItems()
    {
        //instentiate the gameobject and throw them in random directions with a small force to disperse the items
        for (int i = 0; i < items.Length; i++)
        {
            //based on the probability float in the itemBehaviour script, spawn the item
            if (Random.Range(0f, 1f) < items[i].GetComponent<itemBehaviour>().probability)
            {
                GameObject item = Instantiate(items[i], transform.position, Quaternion.identity);
                item.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * 10);
            }
        }
    }
}
