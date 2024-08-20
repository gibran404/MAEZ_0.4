using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerVitals : MonoBehaviour
{

    public Text vitalsText;
    public int health;
    public float stamina;
    public int mana;
    public int sanity;


    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        stamina = 100;
        mana = 100;
        sanity = 100;
    }

    // Update is called once per frame
    void Update()
    {
        updateVitals();


    }

    void updateVitals()
    {
        vitalsText.text = "Health: " + health + "\n" + "Stamina: " + stamina + "\n" + "Mana: " + mana + "\n" + "Sanity: " + sanity;
    }

    public void reduceStamina()
    {
        stamina -= 0.4f;
    }

    public void regenStamina()
    {
        if (stamina < 100)
        {
            stamina += 0.5f;
        }
    }

}
