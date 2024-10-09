using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;

public class NavMeshBaker : MonoBehaviour
{
    public NavMeshSurface navMeshSurface; // Reference to the NavMeshSurface component

    // This will be called when the game starts
    void Start()
    {

        //after 1 second, bake the navmesh
        navMeshSurface.RemoveData();
        StartCoroutine(BakeNavMeshAfterDelay());
    }

    void Update()
    {
        // Bake the NavMesh when the user presses the 'B' key
        if (Input.GetKeyDown(KeyCode.B))
        {
            navMeshSurface.RemoveData();
            BakeNavMesh();
            Debug.Log("NavMesh baked!");
        }
    }

    // Function to bake the NavMesh after a delay
    IEnumerator BakeNavMeshAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        BakeNavMesh();
    }

    // Function to bake the NavMesh
    public void BakeNavMesh()
    {
        navMeshSurface.RemoveData();
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
            Debug.Log("NavMesh baked!");
            //destroy this script
            // Destroy(this);
        }
        else
        {
            Debug.LogError("NavMeshSurface component is not assigned!");
        }
    }
}
