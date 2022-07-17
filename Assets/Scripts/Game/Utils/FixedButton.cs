using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    public Action ButtonPressed;

    [HideInInspector]
    public bool Pressed { get; private set; } = false;

    [HideInInspector]
    public bool Released { get; private set; } = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonPressed?.Invoke();
        Pressed = true;
        Released = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
        Released = true;
    }

}
