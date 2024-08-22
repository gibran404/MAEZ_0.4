using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemsandVitals : MonoBehaviour
{

    [Header("UI Elements")]
    public GameObject healthBar;
    public GameObject stamBar;
    public GameObject manaBar;
    


    [Header("Other")]

    public Text vitalsText;
    public Text healthPotionText;
    public Text manaPotionText;

    // for now, remove later
    public GameObject swordUI;
    public GameObject torchUI;

    public GameObject sword;
    public GameObject torch;
    // for now remove later
    public float health = 100;
    public float stamina = 100;
    public float mana = 100;
    public float sanity = 100;

    public int healthPotionCount = 10;
    public int manaPotionCount = 10;
    public int staminaPotionCount = 10;


    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        stamina = 100;
        mana = 100;
        sanity = 100;

        healthPotionCount = 10;
        manaPotionCount = 10;
    }

    // Update is called once per frame
    void Update()
    {
        updateVitals();

        if (Input.GetKeyDown(KeyCode.H))
        {
            RegenHealth();
            healthPotionCount--;
            healthPotionText.text = healthPotionCount.ToString();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            RegenMana();
            manaPotionCount--;
            manaPotionText.text = manaPotionCount.ToString();
        }

        // remove later
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //flip between sword and torch
            if (sword.activeSelf)
            {
                sword.SetActive(false);
                torch.SetActive(true);

                swordUI.SetActive(false);
                torchUI.SetActive(true);
            }
            else
            {
                sword.SetActive(true);
                torch.SetActive(false);

                swordUI.SetActive(true);
                torchUI.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ReduceHealthBig();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ReduceHealthSmall();
        }

        //remove later
    }
    void updateVitals()
    {
        vitalsText.text = "Health: " + health + "\n" + "Stamina: " + stamina + "\n" + "Mana: " + mana + "\n" + "Sanity: " + sanity + "\n" + "Health Potions: " + healthPotionCount + "\n" + "Mana Potions: " + manaPotionCount + "\n" + "Stamina Potions: " + staminaPotionCount;
    }

    public void ReduceHealthBig()
    {
        health -= 30;
        healthBar.GetComponent<VitalBar>().vitalDeduct(30);
        if (health < 0)
        {
            health = 0;
        }
    }

    public void ReduceHealthSmall()
    {
        health -= 10;
        healthBar.GetComponent<VitalBar>().vitalDeduct(10);
        if (health < 0)
        {
            health = 0;
        }
    }

    public void RegenHealth()
    {
        health += 50;
        healthBar.GetComponent<VitalBar>().vitalRegen(50);
        if (health > 100)
        {
            health = 100;
        }
    }

    public void ReduceMana()
    {
        mana -= 10;
        manaBar.GetComponent<VitalBar>().vitalDeduct(10);
        if (mana < 0)
        {
            mana = 0;
        }
    }

    public void RegenMana()
    {
        mana += 50;
        manaBar.GetComponent<VitalBar>().vitalRegen(50);
        if (mana > 100)
        {
            mana = 100;
        }
    }

    public void reduceStamina() // called from player controller script
    {
        stamina -= 0.4f;
        stamBar.GetComponent<VitalBar>().vitalDeduct(0.4f);
    }

    public void regenStamina() // called from player controller script
    {
        if (stamina < 100)
        {
            stamina += 0.5f;
            stamBar.GetComponent<VitalBar>().vitalRegen(0.5f);
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
