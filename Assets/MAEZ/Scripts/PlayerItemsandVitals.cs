using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemsandVitals : MonoBehaviour
{

    public Text vitalsText;
    public float health;
    public float stamina;
    public float mana;
    public float sanity;


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

    public void ReduceHealthBig()
    {
        health -= 40;
        if (health < 0)
        {
            health = 0;
        }
    }

    public void ReduceHealthSmall()
    {
        health -= 15;
        if (health < 0)
        {
            health = 0;
        }
    }

    public void RegenHealth()
    {
        health += 50;
        if (health > 100)
        {
            health = 100;
        }
    }

    public void ReduceMana()
    {
        mana -= 10;
        if (mana < 0)
        {
            mana = 0;
        }
    }

    public void RegenMana()
    {
        mana += 50;
        if (mana > 100)
        {
            mana = 100;
        }
    }

    public void reduceStamina() // called from player controller script
    {
        stamina -= 0.4f;
    }

    public void regenStamina() // called from player controller script
    {
        if (stamina < 100)
        {
            stamina += 0.5f;
        }
    }

    //function that checks if player is in light or darkness
    // public void checkSanity(bool isLight)
    // {
    //     if (isLight)
    //     {
    //         if (sanity < 100)
    //         {
    //             sanity += 0.1f;
    //         }
    //     }
    //     else
    //     {
    //         sanity -= 0.1f;
    //     }
    // }
}
