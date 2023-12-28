using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;


public class slotGenerationSync : Bolt.EntityBehaviour<ISlotState>
{
    //define slot index number
    public int mySlotID;
   
    private MeshRenderer myMesh;

    private GameObject currentPlayerInSlot;

    //SERIAL DATA
    public int matrixX;
    public int matrixY;
    //Assign color to display on arduino
    private Vector3 rgb;
    //serial collision
    public GameObject[] DetectionSpheres;
    public int OccupiedSeedsCount;

    //Serial States
    private bool PlayerReaction;

    //GENERATION

    //command array
    public enum randomTypes{ JustGrass, HasMoreFlower, HasMoreMushrooms, HasMoreOthers, TotalRandom };
    public randomTypes generationCommand;
 
    //Random slot
    public bool isRandom;

    private void Awake()
    {
        //set slot ID by name
        string slotName = gameObject.name;
        string[] splitNameArr = slotName.Split(char.Parse("("));
        string splitName;
        if (splitNameArr.Length > 1)
        {
            splitName = splitNameArr[1];
            string[] splitNameTwiceArr = splitName.Split(char.Parse(")"));
            string splitNameTwice = splitNameTwiceArr[0];
            mySlotID = int.Parse(splitNameTwice);
        }
        else
        {
            mySlotID = 0;
        }

        matrixX = mySlotID / 16;
        matrixY = mySlotID % 16;

        //generate slot types if it is random
        if(isRandom)
        {
            GenerateSlotTypes();
        }
        
    }
    private void Start()
    {
        //turn off mesh renderer
        myMesh = GetComponent<MeshRenderer>();
        myMesh.enabled = false;
    }
    //sync location
    public override void Attached()
    {
        base.Attached();

        //sync transform state
        state.SetTransforms(state.SlotTransform, gameObject.transform);

    }

    
    public void CheckHitBy(string playerTag)
    {
        
        GameObject hitByPlayer;
        hitByPlayer = GameObject.FindGameObjectWithTag(playerTag);

    }

    private void GenerateSlotTypes()
    {
        //Generate index
        int randomIndex = Random.Range(0, 13);

        if (randomIndex == 0 || randomIndex == 1 || randomIndex == 2)
        {
            generationCommand = randomTypes.JustGrass;
        }
        else if (randomIndex == 3 || randomIndex == 4 || randomIndex == 5)
        {
            generationCommand = randomTypes.HasMoreFlower;
        }
        else if (randomIndex == 6 || randomIndex == 7 || randomIndex == 8)
        {
            generationCommand = randomTypes.HasMoreMushrooms;
        }
        else if (randomIndex == 9 || randomIndex == 10)
        {
            generationCommand = randomTypes.HasMoreOthers;
        }
        else if (randomIndex == 11 || randomIndex == 12)
        {
            generationCommand = randomTypes.TotalRandom;
        }
    }

    //HARDWARE
    //SERIAL COMMUNICATION FUNCTIONS TO ARDUINO EVENT HANDLER

    //highlighted when stooped on
     void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.tag == "Player_Client" || other.gameObject.tag == "Player_Server" )
        {
            PlayerReaction = true;

            //display new color
            if (other.gameObject.tag == "Player_Client")
            {
               
                //send message to arduino event handler:highlight
                rgb = new Vector3(0, 0, 255);
                ArduinoEventHandler.currentEvent.colorHighlighted(matrixX, matrixY, rgb);

                //assign player
                currentPlayerInSlot = GameObject.FindWithTag("Player_Client");

            }
            else if (other.gameObject.tag == "Player_Server")
            {
                //send message to arduino event handler:highlight
                rgb = new Vector3(255, 0, 0);
                ArduinoEventHandler.currentEvent.colorHighlighted(matrixX, matrixY, rgb);

                //assign player
                currentPlayerInSlot = GameObject.FindWithTag("Player_Server");
            }

        }

    }

    //dehighlighted when stooped off
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player_Client" || other.gameObject.tag == "Player_Server")
        {
            //send message to arduino event handler:dehighlight
            rgb = new Vector3(0, 0, 0);
            ArduinoEventHandler.currentEvent.colorDehighlighted(matrixX, matrixY, rgb);

            currentPlayerInSlot = null;

            PlayerReaction = false;
        }
    }

    private void Update()
    {
        //Arduino Functions
        
        CallingPartnerToRipple();
       
    }

    public void AddOneSeedOccupied()
    {
        OccupiedSeedsCount += 1;
    }

    public void RemoveOneSeedOccupied()
    {
        OccupiedSeedsCount -= 1;
    }

    public void CheckSeedsOccupied()
    {
        if (OccupiedSeedsCount > 0)
        {
            //send message to arduino event handler:dehighlight
            rgb = new Vector3(0, 255, 0);
            ArduinoEventHandler.currentEvent.colorHighlighted(matrixX, matrixY, rgb);
        }
        else 
        {
            //send message to arduino event handler:dehighlight
            rgb = new Vector3(0, 0, 0);
            ArduinoEventHandler.currentEvent.colorDehighlighted(matrixX, matrixY, rgb);
        }
    }

    public void CallingPartnerToRipple()
    {
        if (currentPlayerInSlot != null)
        {
            //test mode
            if (currentPlayerInSlot.GetComponent<TestPlayerMovement>() != null)
            {
                if (currentPlayerInSlot.GetComponent<TestPlayerMovement>().isCallingPartner)
                {
                    //send message to arduino event handler:dehighlight
                    rgb = new Vector3(255, 255, 255);
                    ArduinoEventHandler.currentEvent.Ripple(matrixX, matrixY, rgb);
                }
            }
            //OVR mode
            else if (currentPlayerInSlot.GetComponent<OVRMainPlayerSync>() != null)
            {
                if (currentPlayerInSlot.GetComponent<OVRMainPlayerSync>().state.IsCallingPartner)
                {
                    //send message to arduino event handler:dehighlight
                    rgb = new Vector3(255, 255, 255);
                    ArduinoEventHandler.currentEvent.Ripple(matrixX, matrixY, rgb);
                }
            }
            //FPC mode
            else if (currentPlayerInSlot.GetComponent<MainPlayerSync>() != null)
            {
                if (currentPlayerInSlot.GetComponent<MainPlayerSync>().state.IsCallingPartner)
                {
                    //send message to arduino event handler:dehighlight
                    rgb = new Vector3(255, 255, 255);
                    ArduinoEventHandler.currentEvent.Ripple(matrixX, matrixY, rgb);
                }
            }
        }
    }

}
