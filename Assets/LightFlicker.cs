using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Vector3 startingPosition;
    public float flickerIntensity = 0.2f;
    public float flickerPerSecond = 3.0f;
    public float speedRandomness = 1.0f;

    private float time;
    private float startingIntensity;
    private Light light;

    // Update is called once per frame
    void Start()
    {
        light = GetComponent<Light>();
        startingIntensity = light.intensity;
        startingPosition = transform.position;
    }

    void Update()
    {
        // time += Time.deltaTime;
        // light.intensity = startingIntensity + Mathf.Sin(time * flickerPerSecond) * flickerIntensity;
    
        time += Time.deltaTime * (1 - UnityEngine.Random.Range(-speedRandomness, speedRandomness)) * Mathf.PI;
        light.intensity = startingIntensity + Mathf.Sin(time * flickerPerSecond) * flickerIntensity;

        // move light around
        transform.position = new Vector3(
            startingPosition.x + 0.015f * Mathf.Cos(Time.time),
            startingPosition.y + 0.01f * Mathf.Cos(Time.time),
            startingPosition.z + 0.005f * Mathf.Sin(Time.time)
        );
    
    }
}
