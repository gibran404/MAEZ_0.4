using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalBar : MonoBehaviour
{
    public Slider vitalSlider;
    public Slider easeVitalSlider;
    public float maxVital = 100f;
    public float vital;
    private float lerpSpeed = 0.05f;


    // Update is called once per frame
    void Update()
    {
        if (vitalSlider.value != vital)
        {
            if (vital > maxVital)
            {
                vital = maxVital;
            }
            else if (vital <= 0)
            {
                vital = 0;
            }
            vitalSlider.value = vital;
            
        }

        if (vitalSlider.value != easeVitalSlider.value)
        {
            easeVitalSlider.value = Mathf.Lerp(easeVitalSlider.value, vital,lerpSpeed);
        }
    }

    public void vitalDeduct(float damage)
    {
        vital -= damage;
    }

    public void vitalRegen(float regen)
    {
        vital += regen;
    }

    public void setMaxVital()
    {
        vital = maxVital;
    }

    public void setZeroVital()
    {
        vital = 0;
    }
    public void setVital(float newVital)
    {
        vital = newVital;
    }
}
