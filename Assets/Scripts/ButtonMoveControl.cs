using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonMoveControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent MouseDownEvent;
    public UnityEvent MouseUpEvent;


    public void OnPointerDown(PointerEventData eventData)
    {
        MouseDownEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MouseUpEvent.Invoke();
    }
}
