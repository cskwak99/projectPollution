using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnButtonClick();
    public static event OnButtonClick onButtonClick;
    public void RaiseOnButtonClick()
    {
        if (onButtonClick != null)
        {
            onButtonClick();
        }
    }
}