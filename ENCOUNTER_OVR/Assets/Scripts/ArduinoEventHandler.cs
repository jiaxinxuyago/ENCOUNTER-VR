using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoEventHandler : MonoBehaviour
{
    //declare singleton as this serial event handler
    public static ArduinoEventHandler currentEvent;


    //define color highlited event
    public event Action<int, int, Vector3> OnColorHighlighted;

    //define color dehighlighted event
    public event Action<int, int, Vector3> OnColorDehighlighted;

    //define ripple event
    public event Action<int, int, Vector3> OnRipple;

    //call on awake
    private void Awake()
    {
        currentEvent = this;
    }

    //check if color is actually highlighted
    public void colorHighlighted (int x, int y, Vector3 rgb)
    {
        if (OnColorHighlighted != null)
        {
            OnColorHighlighted(x, y, rgb);
        }
    }

    //check if color is actually Dehighlighted
    public void colorDehighlighted(int x, int y, Vector3 rgb)
    {
        if (OnColorDehighlighted != null)
        {
            OnColorDehighlighted(x, y, rgb);
        }
    }

    //check if color is actually Dehighlighted
    public void Ripple(int x, int y, Vector3 rgb)
    {
        if (OnRipple != null)
        {
            OnRipple(x, y, rgb);
        }
    }


}
