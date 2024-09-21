using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrb : MonoBehaviour
{
    public float radius = 1.0f;

    void Update()
    {

        // orbit this object's child around this gameobject in a given radius
        transform.GetChild(0).transform.position = new Vector3(
            transform.position.x + radius * Mathf.Cos(Time.time),
            transform.position.y,
            transform.position.z + radius * Mathf.Sin(Time.time)
        );
    }
}
