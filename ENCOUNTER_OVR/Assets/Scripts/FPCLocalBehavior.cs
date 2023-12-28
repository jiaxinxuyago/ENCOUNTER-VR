using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCLocalBehavior : MonoBehaviour
{
    //initialize body parts
    [SerializeField] private GameObject playerHead;
    [SerializeField] private GameObject playerHand;
    [SerializeField] private GameObject firstPersonCharacter;

    //initialize calling sphere
    [SerializeField] private GameObject callingSphere;

    //get body parts
    private GameObject playerCamera;
    private MeshRenderer playerHandRenderer;

    //get beam shooter
    [HideInInspector] public GameObject beam;
    [HideInInspector] public LineRenderer beamLine;
    [HideInInspector] public beamShooterLocalBehavior beamShooterLocal;

    //CALLBACK STATES
    [HideInInspector] public bool beamShowingState;
    [HideInInspector] public bool energyDepletionState;
    [HideInInspector] public bool isHittingSeedState;
    

    //initailize body parts transform data
    private Quaternion handInitalRotation;

    //initialize input values

    //FPC Group
    [HideInInspector] public bool handsToggle_ON;
    [HideInInspector] public bool handsToggle_OFF;
    [HideInInspector] public bool beamShootingInput_FPC_ON;
    [HideInInspector] public bool beamShootingInput_FPC_OFF;

    //OVR Group
    [HideInInspector] public float beamShootingInput_OVR;

    [HideInInspector] public bool isShooting_FPC;
    [HideInInspector] public bool isShooting_OVR;
    [HideInInspector] public bool isCallingPartner_FPC;
    [HideInInspector] public bool isGrabbingCharger_FPC;

    void Start()
    {
        //arm
        //hide it when start
            playerHandRenderer = playerHand.GetComponent<MeshRenderer>();
            playerHandRenderer.enabled = false;
            handInitalRotation = playerHand.transform.localRotation;

        //camera
            //find player's camera
            playerCamera = firstPersonCharacter;

        //beam shooter
            beam = transform.Find("BeamShooter").gameObject;
            beamShooterLocal = beam.GetComponent<beamShooterLocalBehavior>();
            beamLine = beam.GetComponent<LineRenderer>();
    }

   
    private void Update()
    {
        //beam showing on/off
        beamShowingState = beamLine.enabled;
        //energy depletion
        energyDepletionState = beamShooterLocal.isDepeleted;
        //hitting seed state
        isHittingSeedState = beamShooterLocal.isHittingSeed;
    }

    //get input values
    public void InputValues()
    {
        //initialize FPC Data

        //hands input
        handsToggle_ON = Input.GetMouseButtonDown(0);
        handsToggle_OFF = Input.GetMouseButtonUp(0);

        //beam shooting input
        beamShootingInput_FPC_ON = Input.GetMouseButtonDown(1);
        beamShootingInput_FPC_OFF = Input.GetMouseButtonUp(1);

        //calling input
        isCallingPartner_FPC = Input.GetKeyDown(KeyCode.Return);

        //grabbing input
        isGrabbingCharger_FPC = Input.GetKeyDown(KeyCode.E);
    }

    //trigger beam
    public void beamInput()
    {
        //MOUSE _ FIRST PERSON MODE

        if (beamShootingInput_FPC_ON)
        {
            isShooting_FPC = true;
        }
        else if (beamShootingInput_FPC_OFF)
        {
            isShooting_FPC = false;
        }


        //TRIGGER _ OVR MODE

        if (beamShootingInput_OVR > 0.1)
        {
            isShooting_OVR = true;
        }
        else if (beamShootingInput_OVR < 0.1)
        {
            isShooting_OVR = false;
        }
    }

    //calling partner
    public void callingPartner()
    {
        if (isCallingPartner_FPC)
        {
            BoltNetwork.Instantiate(callingSphere, playerHead.transform.position, Quaternion.identity);
        }
    }

    //Arm Behavior _ Owner
    public void ownerHandBehavior()
    {
        //pack rotation and toggling together
        toggleHands(playerHandRenderer);
        rotateArm(playerHand, playerHandRenderer);   
    }

    

    //Head Behavior
    public void headBehavior()
    {
        //rotation of head
        rotateHead();
    }

    //head rotation
    void rotateHead()
    {
        playerHead.transform.localRotation = playerCamera.transform.localRotation;
    }

    

    //hands showing up and off
    void toggleHands(MeshRenderer handTargetToShow)
    {
        // toggle hand showing up and off
        if (handsToggle_ON)
        {
            handTargetToShow.enabled = true;

        }
        else if (handsToggle_OFF)
        {
            handTargetToShow.enabled = false;

        }
    }

    

    //arm rotation
    void rotateArm(GameObject handTarget, MeshRenderer handTargetToShow)
    {
        if (handTargetToShow.enabled == false)
        {
            //get initial rotation of hand_not owner
            handTarget.transform.localRotation = handInitalRotation;
        }
        else
        {
            //rotate arm with head point direction
            handTarget.transform.localRotation = playerCamera.transform.localRotation;
        }

    }

    
}
