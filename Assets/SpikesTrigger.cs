using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTrigger : MonoBehaviour
{
    public GameObject Spikes;
    public bool triggered = false;
    private Vector3 SpikesDownPos;

    public AudioClip SpikesSound;

    // Start is called before the first frame update
    void Start()
    {
        SpikesDownPos = Spikes.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if spikes are triggered, after 2 seconds, spikes will be moved back down by 2
        
    }

    //when player comes into contact, spikes will be moved up by 2
    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player" || other.tag == "Enemy") && !triggered)
        {
            Debug.Log("Spikes Triggered");
            triggered = true;
            if (Spikes.transform.position == SpikesDownPos)
            {
                
                transform.position += new Vector3(0, -0.05f, 0);
                StartCoroutine(ActivateSpikes());
            }
        }
    }

    //after 2 seconds, spikes will be moved up by 2
    IEnumerator ActivateSpikes()
    {
        yield return new WaitForSeconds(0.5f);

        AudioSource.PlayClipAtPoint(SpikesSound, transform.position);
        Debug.Log("Spikes Activated");
        Spikes.transform.position += new Vector3(0, 2, 0);
        StartCoroutine(ResetSpikes());
    }

    //after 2 seconds, spikes will be moved back down by 2
    IEnumerator ResetSpikes()
    {
        yield return new WaitForSeconds(0.25f);
        Debug.Log("Spikes Reset");
        triggered = false;
        Spikes.transform.position += new Vector3(0, -2, 0);
        transform.position += new Vector3(0, 0.05f, 0);

    }
}
