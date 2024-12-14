using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateBar : MonoBehaviour
{
    public Slider UltimateBarSlider;

    public void UltimateReady(float damage)
    {
        UltimateBarSlider.maxValue = damage;
        UltimateBarSlider.value = damage;
    }

    public void SetCharge(float damage)
    {
        UltimateBarSlider.value = damage;
    }

    public void ResetCharge(float damage)
    {
        UltimateBarSlider.value = damage;
    }
}
