using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject NewGamePanel;
    public GameObject LoadPanel;

    void Start()
    {
        //fps locked at 60 for consistency
        Application.targetFrameRate = 60;
    }

    public void NewGame()
    {
        MainMenuPanel.SetActive(false);
        NewGamePanel.SetActive(true);
    }

    public void LoadGame()
    {
        MainMenuPanel.SetActive(false);
        LoadPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        MainMenuPanel.SetActive(true);
        NewGamePanel.SetActive(false);
        LoadPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMaez()
    {
        //initialize the playeritems and vitals to default values
    //     // public static bool isplayerAlive = true;
    // public static float health = 100;
    // public static float stamina = 100;
    // public static float mana = 100;
    // public static float sanity = 100;
    // public static int healthPotionCount = 10;
    // public static int manaPotionCount = 10;

        
        PlayerItemsandVitals.isplayerAlive = true;
        PlayerItemsandVitals.health = 100;
        PlayerItemsandVitals.stamina = 100;
        PlayerItemsandVitals.mana = 100;
        PlayerItemsandVitals.sanity = 100;
        PlayerItemsandVitals.healthPotionCount = 10;
        PlayerItemsandVitals.manaPotionCount = 10;

        UnityEngine.SceneManagement.SceneManager.LoadScene("MAEZ");
    }

    public void LoadIntro()
    {
        // time scale to 1
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Intro");
    }
    
}
