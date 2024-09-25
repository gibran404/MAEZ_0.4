using UnityEngine;

public class FloatingWallDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Check immediately when the object is spawned
        CheckCollisionOnSpawn();
    }

    void CheckCollisionOnSpawn()
    {
        // Get all colliders overlapping the trigger area
        Collider[] hitColliders = Physics.OverlapBox(
            transform.position, 
            GetComponent<BoxCollider>().size / 2, 
            transform.rotation
        );

        // Flag to track if a "Structure" is detected
        bool structureDetected = false;

        // Loop through the colliders and check for any tagged "Structure"
        foreach (Collider collider in hitColliders)
        {
            if ( collider.CompareTag("Floor")) // collider.CompareTag("Structure") ||
            {
                Debug.Log("Structure detected, bool flipped");
                structureDetected = true;
                break; // No need to check further if we found a structure
            }
        }

        // If no "Structure" was detected, destroy the GameObject
        if (!structureDetected)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Structure detected, not destroying wall");
        }
    }

    // // Optional: Add this to check for collisions during gameplay
    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Structure"))
    //     {
    //         // You can add logic here if needed during runtime
    //     }
    // }
}
