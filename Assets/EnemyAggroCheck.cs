using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    private bool playerInRange = false;

    [SerializeField]
    public float aggroTime = 3f;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<EnemyFollow>().Aggro = true;
            playerInRange = true;
            StopAllCoroutines();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<EnemyFollow>().Aggro = true;
            playerInRange = true;
            StopAllCoroutines();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        if (!playerInRange)
        {
            StartCoroutine(DisableAggro());
        }
        

        // if player is dead, disable aggro
        if (PlayerItemsandVitals.health <= 0)
        {
            GetComponentInParent<EnemyFollow>().Aggro = false;
        }
    }

    IEnumerator DisableAggro()
    {
        yield return new WaitForSeconds(aggroTime);
        GetComponentInParent<EnemyFollow>().Aggro = false;
    }
}
