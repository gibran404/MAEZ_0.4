using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariantRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] walls;
    void Start()
    {
        //randomly pick one of the walls and destroy the rest
        int randomWall = Random.Range(0, walls.Length);
        walls[randomWall].SetActive(true);
        for (int i = 0; i < walls.Length; i++)
        {
            if (i != randomWall)
            {
                Destroy(walls[i]);
            }
        }
    }
}
