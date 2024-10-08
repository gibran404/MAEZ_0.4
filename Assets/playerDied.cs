using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class playerDied : MonoBehaviour
{
    public GameObject Player;
    public GameObject playerDiedUI;
    public GameObject VitalBars;
    public GameObject Hotbar;
    // Start is called before the first frame update
    void Start()
    {
        Player.GetComponent<ThirdPersonController>().enabled = false;

        VitalBars.SetActive(false);
        Hotbar.SetActive(false);
        Player.GetComponent<Animator>().SetBool("Dead", true);
        GetComponent<PlayerItemsandVitals>().enabled = false;

        StartCoroutine(ShowPlayerDiedUI());
    }

    // Update is called once per frame
    void Update()
    {
        // if animator is on DeathB state
        // if (Player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Death_B"))
        // {
        //     Player.GetComponent<Animator>().SetBool("Dead", false);
        // }
    }

    IEnumerator ShowPlayerDiedUI()
    {
        yield return new WaitForSeconds(2);
        playerDiedUI.SetActive(true);
    }
}
