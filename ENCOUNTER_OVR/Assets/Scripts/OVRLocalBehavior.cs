using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRLocalBehavior : MonoBehaviour
{
    //initialize body parts
    [SerializeField] private GameObject playerHead;
    [SerializeField] private GameObject playerLeftHand;
    [SerializeField] private GameObject playerRightHand;

    //initialize calling sphere
    [SerializeField] private GameObject callingSphere;

    //get beam shooter
    [HideInInspector] public GameObject beam;
    [HideInInspector] public LineRenderer beamLine;
    [HideInInspector] public beamShooterLocalBehavior beamShooterLocal;

    //CALLBACK STATES
    [HideInInspector] public bool beamShowingState;
    [HideInInspector] public bool energyDepletionState;
    [HideInInspector] public bool isHittingSeedState;

    //initialize input values

    //OVR Group
    [HideInInspector] public float beamShootingInput_OVR;
    [HideInInspector] public bool isShooting_OVR;
    [HideInInspector] public bool isCallingPartner_OVR;
    [HideInInspector] public float isGrabbingCharger_OVR;

    // Start is called before the first frame update
    void Start()
    {
        //beam shooter
        beam = transform.Find("BeamShooter").gameObject;
        beamShooterLocal = beam.GetComponent<beamShooterLocalBehavior>();
        beamLine = beam.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
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

        //beam shooting input

        // returns a float of the left index finger trigger’s current state.
        // (range of 0.0f to 1.0f)
        beamShootingInput_OVR = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger); 
       
        //calling input
        isCallingPartner_OVR = OVRInput.GetDown(OVRInput.RawButton.A);

        //grabbing input
        isGrabbingCharger_OVR = OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger);
    }

    //trigger beam
    public void beamInput()
    {
        //TRIGGER _ OVR MODE

        if (beamShootingInput_OVR > 0.1 && isGrabbingCharger_OVR < 0.1)
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
        if (isCallingPartner_OVR)
        {
            BoltNetwork.Instantiate(callingSphere, playerHead.transform.position, Quaternion.identity);
        }
    }

    public void CheckIsGrabbing(GameObject grabbingObject)
    {
        if (isGrabbingCharger_OVR > 0)
        {
            grabbingObject.SendMessage("GrabbingDecay");
        }
    }

    public void RechargeEnergySphere()
    {
        beamShooterLocal.Recharge();
    }
}
