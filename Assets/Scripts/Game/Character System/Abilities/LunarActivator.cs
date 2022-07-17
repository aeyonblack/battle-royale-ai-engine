using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunarActivator : MonoBehaviour
{
    public FixedButton ActivateButton;
    public GameObject LunarEffect;
    private float duration;

    private void Start()
    {
        LunarEffect.SetActive(false);
        OnEffectStart();
        OnEffectEnded();
    }

    private void OnEffectEnded()
    {
        LunarEffect.GetComponent<LunarEffect>().EffectEnded += () =>
        {
            LunarEffect.SetActive(false);
        };
    }

    private void OnEffectStart()
    {
        ActivateButton.ButtonPressed += () =>
        {
            if (!LunarEffect.activeSelf) LunarEffect.SetActive(true);
        };
    }
}
