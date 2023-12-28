using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class ArduinoCommunication : MonoBehaviour
{
    //check pin number of Neopixel matrix from arduino
    [HideInInspector] public int LEDMatrixPinNum;
    
    // Start is called before the first frame update
    void Start()
    {

        //set up pin mode here
        UduinoManager.Instance.pinMode(LEDMatrixPinNum, PinMode.Output);


        //subscribe to arduino event handler
        ArduinoEventHandler.currentEvent.OnColorHighlighted += turnOnThisPixel;
        ArduinoEventHandler.currentEvent.OnColorDehighlighted += turnOffThisPixel;
        ArduinoEventHandler.currentEvent.OnRipple += RipplePixels;


    }

    // Update is called once per frame
    void Update()
    {
        
        //debug function
        toggleByKey();

        if (!Application.isPlaying)
        {
            //turn off the whole matrix board
            UduinoManager.Instance.sendCommand("tOffB");
        }
    }

    //turn on this LED
    public void turnOnThisPixel(int x, int y, Vector3 rgb)
    {
       
        //set high
        UduinoManager.Instance.sendCommand("tOnPx", x, y, rgb.x, rgb.y, rgb.z);

    }

    //turn off this LED
    public void turnOffThisPixel(int x, int y, Vector3 rgb)
    {

        //set low
        UduinoManager.Instance.sendCommand("tOffPx", x, y);

    }

    //Ripple
    public void RipplePixels(int x, int y, Vector3 rgb)
    {
        UduinoManager.Instance.sendCommand("Rp", x, y, rgb.x, rgb.y, rgb.z);
    }

    //make manual toggle function for debugging
    public void toggleByKey ()
    {

        //press P to switch on
        if (Input.GetKeyDown(KeyCode.P))
        {
            //turn on the whole matrix board
            UduinoManager.Instance.sendCommand("tOnB");
        }

        //press O to switch off
        if (Input.GetKeyDown(KeyCode.O))
        {
            //turn off the whole matrix board
            UduinoManager.Instance.sendCommand("tOffB");
        }
    }

    

}
