using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFPCHandBehavior : MonoBehaviour
{
    //get arm parts
    private GameObject playerArm;
    
    private GameObject playerCamera;



    // Start is called before the first frame update
    void Start()
    {
        //find player's arm
        playerArm = transform.Find("Arm").gameObject;
        
        //find player's camera
        playerCamera = transform.Find("FirstPersonCharacter").gameObject;

        //hide it when start
        playerArm.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        toggleHands();
        rotateArm();

    }

    void toggleHands()
    {
        // toggle hand showing up and off
        if (Input.GetMouseButtonDown(0))
        {
            playerArm.SetActive(true);

        }
        else if (Input.GetMouseButtonUp(0))
        {
            playerArm.SetActive(false);

        }
    }

    void rotateArm()
    {
        playerArm.transform.localRotation = playerCamera.transform.localRotation;
    }
}
