using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;  // Drag your pause menu UI here
    public static bool isPaused = false;

    void Start()
    {
        // Hide cursor and lock it in the center of the game window at the start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ensure the pause menu is hidden at the start
        pauseMenu.SetActive(false);

        ResumeGame();
    }

    void Update()
    {
        // Check for ESC key press
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        // Show the cursor and unlock it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Show pause menu
        pauseMenu.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        // Hide the cursor and lock it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Hide pause menu
        pauseMenu.SetActive(false);

        // Resume the game
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void HighSettings()
    {
        QualitySettings.SetQualityLevel(2);
        Debug.Log("High Settings Set");
    }

    public void MediumSettings()
    {
        QualitySettings.SetQualityLevel(1);
        Debug.Log("Medium Settings Set");
    }

    public void LowSettings()
    {
        QualitySettings.SetQualityLevel(0);
        Debug.Log("Low Settings Set");
    }

}
