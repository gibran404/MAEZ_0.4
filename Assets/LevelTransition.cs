using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    bool playerInZone;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the level transition trigger");
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the level transition trigger");
            playerInZone = false;
        }
    }
    
    void NextLevel()
    {
        //increase dungeon size every time the player goes to the next level
        // a 50-50 chance the size increases
        if (Random.Range(0, 2) == 0)
        {
            GameVariables.DungeonSize += new Vector2Int(1, 1);
        }            
        //reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInZone)
        {
            NextLevel();
        }
        if (Input.GetKeyDown(KeyCode.N) && Input.GetKey(KeyCode.LeftControl))
        {
            NextLevel();
        }
    }
}
