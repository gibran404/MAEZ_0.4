using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUnhideMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //if scene name is MAEZ, hide mouse else unhide mouse
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MAEZ")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; 
        }

    }

    // Update is called once per frame
    // void Update()
    // {
    //     //if alt pressed, switch between hide and unhide to interact with pause menu
        
    //     if (Input.GetKeyDown(KeyCode.LeftAlt))
    //     {
    //         Cursor.visible = !Cursor.visible;
    //         Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    //     }
    // }
}
