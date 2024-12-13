using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateBar : MonoBehaviour
{
    public Slider UltimateBarSlider;

    public void UltimateReady(float readyness)
    {
        UltimateBarSlider.maxValue = readyness;
        UltimateBarSlider.value = readyness;
    }

    public void ResetCharge(float readyness)
    {
        UltimateBarSlider.value = readyness;
    }
}
