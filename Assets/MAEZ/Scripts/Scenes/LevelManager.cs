using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReGenerate()
    {
        // Regenerate the level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}


[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelManager levelManager = (LevelManager)target;
        if (GUILayout.Button("Regenerate Level"))
        {
            levelManager.ReGenerate();
        }
    }
}
