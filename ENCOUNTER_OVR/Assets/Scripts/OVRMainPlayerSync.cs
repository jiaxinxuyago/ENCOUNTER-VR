using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class OVRMainPlayerSync : Bolt.EntityBehaviour<IMainPlayerState>
{
    [SerializeField] private GameObject playerHead;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject playerLeftHand;
    [SerializeField] private GameObject playerLeftArm;
    [SerializeField] private GameObject playerRightHand;
    [SerializeField] private GameObject playerRightArm;
    [SerializeField] private GameObject OVRCameraRig;
    [SerializeField] private GameObject CenterEyeAnchor;
    [SerializeField] private GameObject CharacterRig;

    [SerializeField] private GameObject playerHead_NotOwner;
    [SerializeField] private GameObject playerBody_NotOwner;
    [SerializeField] private GameObject playerLeftHand_NotOwner;
    [SerializeField] private GameObject playerLeftArm_NotOwner;
    [SerializeField] private GameObject playerRightHand_NotOwner;
    [SerializeField] private GameObject playerRightArm_NotOwner;


    private Collider collider_NotOwner;
    
    [HideInInspector] public OVRLocalBehavior playerLocalBehavior;

    //public delegate void localBehavior();
    //public static localBehavior localBehaviorContainer;

    private void Awake()
    {
        playerLocalBehavior = GetComponent<OVRLocalBehavior>();
        collider_NotOwner = GetComponent<CapsuleCollider>();
    }
    public override void Attached()
    {
        base.Attached();

        //sync player transform data
        state.SetTransforms(state.PlayerTransform, gameObject.transform);
        state.SetTransforms(state.HeadTransform, playerHead.transform);
        state.SetTransforms(state.BodyTransform, playerBody.transform);
        state.SetTransforms(state.LeftArmTransform, playerLeftArm.transform);
        state.SetTransforms(state.LeftHandTransform, playerLeftHand.transform);
        state.SetTransforms(state.RightArmTransform, playerRightArm.transform);
        state.SetTransforms(state.RightHandTransform, playerRightHand.transform);

        state.SetTransforms(state.HeadNotOwnerTransform, playerHead_NotOwner.transform);
        state.SetTransforms(state.BodyNotOwnerTransform, playerBody_NotOwner.transform);
        state.SetTransforms(state.LeftArmNotOwnerTransform, playerLeftArm_NotOwner.transform);
        state.SetTransforms(state.RightArmNotOwnerTransform, playerRightArm_NotOwner.transform);
        state.SetTransforms(state.LeftHandNotOwnerTransform, playerLeftHand_NotOwner.transform);
        state.SetTransforms(state.RightHandNotOwnerTransform, playerRightHand_NotOwner.transform);

        //CALLBACK FUNCTIONS

        //SYNC beam line on/off
        state.AddCallback("IsBeaming", BeamingUpdate);
        state.AddCallback("IsDepleted", BeamingUpdate);
        state.AddCallback("IsCallingPartner", CallingPartnerUpdate);

        //SYNCHRONIZE WHEN IT'S NOT OWNER
        if (!entity.IsOwner)
        {
            //kill controller and scripts
            GetComponent<CharacterController>().enabled = false;
            GetComponent<OVRPlayerController>().enabled = false;
            GetComponent<OVRSceneSampleController>().enabled = false;
            GetComponent<OVRDebugInfo>().enabled = false;
            OVRCameraRig.SetActive(false);
            //CharacterRig.SetActive(false);
            //turn on fake collider
            collider_NotOwner.enabled = true;
            
        }

        //SYNCHRONIZE WHEN IT'S OWNER
        else
        {
            //turn off fake collider
            collider_NotOwner.enabled = true;
            
            //construct local beahvior container
            //localBehaviorContainer += playerLocalBehavior.InputValues;
            //localBehaviorContainer += playerLocalBehavior.beamInput;
            //localBehaviorContainer += playerLocalBehavior.callingPartner;
        }


    }

    public override void SimulateOwner()
    {
        base.SimulateOwner();
    }


    private void Update()
    {
       
        if (entity.IsOwner)
        {
            playerLocalBehavior.InputValues();
            playerLocalBehavior.beamInput();
            playerLocalBehavior.callingPartner();

            playerHead_NotOwner.transform.position = playerHead.transform.position;
            playerHead_NotOwner.transform.rotation = playerHead.transform.rotation;

            playerBody_NotOwner.transform.position = playerBody.transform.position;
            playerBody_NotOwner.transform.rotation = playerBody.transform.rotation;

            playerLeftArm_NotOwner.transform.position = playerLeftArm.transform.position;
            playerLeftArm_NotOwner.transform.rotation = playerLeftArm.transform.rotation;

            playerRightArm_NotOwner.transform.position = playerRightArm.transform.position;
            playerRightArm_NotOwner.transform.rotation = playerRightArm.transform.rotation;

            playerLeftHand_NotOwner.transform.position = playerLeftHand.transform.position;
            playerLeftHand_NotOwner.transform.rotation = playerLeftHand.transform.rotation;

            playerRightHand_NotOwner.transform.position = playerRightHand.transform.position;
            playerRightHand_NotOwner.transform.rotation = playerRightHand.transform.rotation;
        }
        else
        {
            playerHead.GetComponent<MeshRenderer>().enabled = false;
            playerBody.GetComponent<MeshRenderer>().enabled = false;
            playerLeftArm.GetComponent<MeshRenderer>().enabled = false;
            playerRightArm.GetComponent<MeshRenderer>().enabled = false;
            playerLeftHand.GetComponent<MeshRenderer>().enabled = false;
            playerRightHand.GetComponent<MeshRenderer>().enabled = false;
        }

       
        //update states
        BeamingUpdate();
        CallingPartnerUpdate();
    }

    //CALLBACK FUNCTIONS

        //update beaming input
    private void BeamingUpdate()
    {
        if(entity.IsOwner)
        {
            state.IsBeaming = playerLocalBehavior.beamShowingState;
            state.IsDepleted = playerLocalBehavior.energyDepletionState;
            state.IsHittingSeed = playerLocalBehavior.isHittingSeedState;
        }
        else
        {
            playerLocalBehavior.beamLine.enabled = state.IsBeaming;
            playerLocalBehavior.energyDepletionState = state.IsDepleted;
            playerLocalBehavior.beamShooterLocal.isHittingSeed = state.IsHittingSeed;
        }
       
    }

        //update calling input
    private void CallingPartnerUpdate()
    {
        if (entity.IsOwner)
        {
            state.IsCallingPartner = playerLocalBehavior.isCallingPartner_OVR;
        }
    }
    
        
}
