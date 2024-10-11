using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemsandVitals : MonoBehaviour
{
    [Header("Player")]
    public GameObject Player;
    public static bool isplayerAlive = true;
    public static float health = 100;
    public static float stamina = 100;
    public static float mana = 100;
    public static float sanity = 100;
    public static int healthPotionCount = 10;
    public static int manaPotionCount = 10;


    [Header("UI Elements")]
    public GameObject healthBar;
    public GameObject stamBar;
    public GameObject manaBar;

    [Header("Other")]

    public Text vitalsText;
    public Text healthPotionText;
    public Text manaPotionText;

    public GameObject sword;
    public GameObject torch;
    // for now remove later
    

    // mana reduction var for light orb
    private float manaReductionTimer = 0f;
    // private float lastHitTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        manaBar.GetComponent<VitalBar>().setVital(mana);
        healthBar.GetComponent<VitalBar>().setVital(health);
        stamBar.GetComponent<VitalBar>().setVital(stamina);

        // lastHitTime = Time.time;

        // health = 100;
        // stamina = 100;
        // mana = 100;
        // sanity = 100;

        // healthPotionCount = 10;
        // manaPotionCount = 10;

        sword.SetActive(true);
        torch.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        updateVitals();
        if (health <= 0 && isplayerAlive)
        {
            Debug.Log("player died");

            isplayerAlive = false;
            GetComponent<playerDied>().enabled = true;
            Player.GetComponent<Animator>().SetBool("Dead", true);
            // Player.GetComponent<ThirdPersonController>().enabled = false;
            torch.SetActive(false);
            mana = 0;
            health = 0;
            stamina = 0;
            sanity = 0;

            return;
        }

        //mana reduction for light orb every second
        if (torch.activeSelf)
        {
            manaReductionTimer += Time.deltaTime;

            // If 1 second has passed, reduce mana
            if (manaReductionTimer >= 1f && mana > 1)
            {
                PlayerItemsandVitals.mana -= 2;
                manaBar.GetComponent<VitalBar>().vitalDeduct(2);
                manaReductionTimer = 0f;  // Reset timer
            }
            else if (mana <= 1)
            {
                mana = 0;
                manaBar.GetComponent<VitalBar>().vitalDeduct(100);
                FlipTorch();
            }
        }


        if (Input.GetKeyDown(KeyCode.H))
        {
            if (healthPotionCount > 0 && health < 100)
            {
                RegenHealth();
                healthPotionCount--;
                healthPotionText.text = healthPotionCount.ToString();
            }
        }
        if (Input.GetKeyDown(KeyCode.M) && mana < 100)
        {
            if(manaPotionCount > 0)
            {   
                RegenMana();
                manaPotionCount--;
                manaPotionText.text = manaPotionCount.ToString();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //flip between sword and torch
            FlipTorch();
        }

    }

    void FlipTorch()
    {
        

        if (torch.activeSelf)
        {
            torch.SetActive(false);
        }
        else
        {
            torch.SetActive(true);
        }
    }
    void updateVitals()
    {
        healthPotionText.text = healthPotionCount.ToString();
        manaPotionText.text = manaPotionCount.ToString();

        vitalsText.text = "Health: " + health + "\n" + "Stamina: " + stamina + "\n" + "Mana: " + mana + "\n" + "Sanity: " + sanity + "\n" + "Health Potions: " + healthPotionCount + "\n" + "Mana Potions: " + manaPotionCount + "\n" + "FPS: " + (1.0f / Time.deltaTime).ToString("0");
    }

    public void ReduceHealthBig()
    {
        // if (Time.time - lastHitTime < 1f)
        // {
        //     return;
        // }

        health -= 30;
        healthBar.GetComponent<VitalBar>().vitalDeduct(30);
        if (health < 0)
        {
            health = 0;
        }
        // lastHitTime = Time.time;
    }

    public void ReduceHealthSmall()
    {
        // if (Time.time - lastHitTime < 1f)
        // {
        //     return;
        // }

        health -= 15;
        healthBar.GetComponent<VitalBar>().vitalDeduct(15);
        if (health < 0)
        {
            health = 0;
        }
        // lastHitTime = Time.time;

    }

    public void RegenHealth()
    {
        health += 50;
        healthBar.GetComponent<VitalBar>().vitalRegen(50);
        if (health > 100)
        {
            // healthBar.GetComponent<VitalBar>().setMaxHealth();
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
            // manaBar.GetComponent<VitalBar>().setZeroHealth();
        }
    }

    public void RegenMana()
    {
        mana += 50;
        manaBar.GetComponent<VitalBar>().vitalRegen(50);
        if (mana > 100)
        {
            // manaBar.GetComponent<VitalBar>().setMaxHealth();
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
