using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGmusic : MonoBehaviour
{
    //array of audio clips
    public AudioClip[] audioClips;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // randomly play one of the audio clips
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
        
    }

    public void setMaxVol()
    {
        audioSource.volume = 0.5f;
    }

    public void setMedVol()
    {
        audioSource.volume = 0.3f;
    }

    public void setMinVol()
    {
        audioSource.volume = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        //when the audio clip is done playing, play another random audio clip
        if (!audioSource.isPlaying)
        {
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.Play();
        }
    }
}
