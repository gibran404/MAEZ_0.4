using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMaez()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MAEZ");
    }

    public void LoadIntro()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Intro");
    }

}
