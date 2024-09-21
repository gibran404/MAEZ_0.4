using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrb : MonoBehaviour
{
    public float radius = 1.3f;
    void Update()
    {
        // orbit this object's child around this gameobject in a given radius
        if (transform.parent != null)
        {
            transform.position = new Vector3(
            transform.parent.position.x + radius * Mathf.Cos(Time.time),
            transform.parent.position.y,
            transform.parent.position.z + radius * Mathf.Sin(Time.time)
            );
        }
    }

}
