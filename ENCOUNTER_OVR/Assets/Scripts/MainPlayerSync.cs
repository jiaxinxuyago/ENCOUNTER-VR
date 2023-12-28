using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class MainPlayerSync : Bolt.EntityBehaviour<IMainPlayerState>
{
    [SerializeField] private GameObject playerHead;
    [SerializeField] private GameObject playerHand;
    [SerializeField] private GameObject playerHand_NotOwner;
    [SerializeField] private GameObject playerHand_Still;
    [SerializeField] private GameObject firstPersonCharacter;
    private Collider collider_NotOwner;

    public delegate void localBehavior();
    public static localBehavior localBehaviorContainer;
    [HideInInspector] public FPCLocalBehavior playerLocalBehavior;

    
    private void Awake()
    {
        playerLocalBehavior = GetComponent<FPCLocalBehavior>();
        collider_NotOwner = GetComponent<CapsuleCollider>();
    }

    public override void Attached()
    {
        base.Attached();

        //sync player transform data
        state.SetTransforms(state.PlayerTransform, gameObject.transform);
        state.SetTransforms(state.HandTransform, playerHand.transform);
        state.SetTransforms(state.HeadTransform, playerHead.transform);


        //CALLBACK FUNCTIONS

        //SYNC beam line on/off
        state.AddCallback("IsBeaming", BeamingUpdate);
        state.AddCallback("IsDepleted", BeamingUpdate);
        state.AddCallback("IsCallingPartner", CallingPartnerUpdate);

        //SYNCHRONIZE WHEN IT'S NOT OWNER
        if (!entity.IsOwner)
        {
            //kill controller
            firstPersonCharacter.SetActive(false);
            GetComponent<CharacterController>().enabled = false;
            GetComponent<FirstPersonController>().enabled = false;
            
            //show body parts
            playerHand_NotOwner.SetActive(true);
            playerHand_Still.SetActive(true);

            //hide owner parts
            playerHand.SetActive(false);

            //turn on fake collider
            collider_NotOwner.enabled = true;
        }

        //SYNCHRONIZE WHEN IT'S OWNER
        else
        {
            //turn off fake collider
            collider_NotOwner.enabled = false;

            //construct local beahvior container
            localBehaviorContainer += playerLocalBehavior.InputValues;
            localBehaviorContainer += playerLocalBehavior.ownerHandBehavior;
            localBehaviorContainer += playerLocalBehavior.headBehavior;
            localBehaviorContainer += playerLocalBehavior.beamInput;
            localBehaviorContainer += playerLocalBehavior.callingPartner;
        }


    }

    public override void SimulateOwner()
    {
        base.SimulateOwner();
    }


    private void Update()
    {
        localBehaviorContainer();
        
        //simulate hand_notOwner with real hands
        playerHand_NotOwner.transform.rotation = playerHand.transform.rotation;

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
            state.IsCallingPartner = playerLocalBehavior.isCallingPartner_FPC;
        }
    }


}
