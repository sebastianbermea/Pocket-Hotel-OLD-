using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class COPPA : MonoBehaviour
{
    public Slider ageSlider;
    public Text ageT;

    public void OnSlide()
    {
        ageT.text = ageSlider.value.ToString();
        if (ageSlider.value > 49)
            ageT.text = "50+";
    }
    /*public void Confirm()
    {
        Fire.INS.SetCopa((int)ageSlider.value);
        Destroy(gameObject);
    }*/
}
