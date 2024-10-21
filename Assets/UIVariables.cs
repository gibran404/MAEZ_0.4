using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVariables : MonoBehaviour
{
    [SerializeField]
    public bool UiEnabled;
    public static bool isUiEnabled;
    public static float MoveSpeed = 2.0f;
    public static bool SprintFlag = false;

    public static bool UIAttacking = false;
    public static bool UIBlocked = false;
    public static bool UIE = false;

    public GameObject UI;

    public GameObject ISOCam;



    void Start()
    {
        isUiEnabled = UiEnabled;

        if (UiEnabled)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            ISOCam.GetComponent<OrbitCameraController>().enabled = false;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            UI.SetActive(false);
        }
    }

    void Update()
    {





        // if ui is enables but the cursor is locked, 
        // unlock the cursor and make it visible
        if (isUiEnabled)
        {
            UI.SetActive(true);

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            UI.SetActive(false);
            // Cursor.visible = false;
            // Cursor.lockState = CursorLockMode.Locked;
        }
        // if ui is disabled but the cursor is unlocked,
        // lock the cursor and make it invisible
        // else if (!isUiEnabled && Cursor.lockState == CursorLockMode.None)
        // {
        //     Cursor.visible = false;
        //     Cursor.lockState = CursorLockMode.Locked;
        // }

        //if player dead, disable all of the UI
        if (PlayerItemsandVitals.isplayerAlive == false)
        {
            UI.SetActive(false);
        }

        if (PauseMenuController.isPaused && UI.activeSelf)
        {
            UI.SetActive(false);
        }
        else if (!PauseMenuController.isPaused && !UI.activeSelf && isUiEnabled)
        {
            UI.SetActive(true);
        }

    }

    public void Attackon()
    {
        UIAttacking = true;
        Debug.Log("Attackon");
    }
    public void Attackoff()
    {
        UIAttacking = false;
        Debug.Log("Attackoff");
    }

    public void Blockon()
    {
        UIBlocked = true;
    }
    public void Blockoff()
    {
        UIBlocked = false;
    }

    public void Eon()
    {
        UIE = true;
        
    }
    public void Eoff()
    {
        UIE = false;
    }



}


