using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beamShooterSync : Bolt.EntityBehaviour<IBeamShooterState>
{
    private GameObject parentGameObject;
    private BoltEntity parentEntity;
    private beamShooterLocalBehavior beamShooterLocal;
    private GameObject playerhand;
    private Transform hitPoint;
    private GameObject beamEnergySphere;

    private void Start()
    {
        //set parent object
        parentGameObject = transform.parent.gameObject;
       
        parentEntity = parentGameObject.GetComponent<BoltEntity>();
        beamShooterLocal = GetComponent<beamShooterLocalBehavior>();
        playerhand = beamShooterLocal.playerHand;
        beamEnergySphere = gameObject.transform.GetChild(0).gameObject;

    }

    public override void Attached()
    {
        base.Attached();

        //sync shooter transform data
        state.SetTransforms(state.ShooterTransform, gameObject.transform);
        state.SetTransforms(state.HitTransform, hitPoint.transform);
        state.SetTransforms(state.HandTransform, playerhand.transform);
        state.SetTransforms(state.EnergySphereTransform, beamEnergySphere.transform);
    }


    private void FixedUpdate()
    {

        //SYNCHRONIZE WHEN IT'S OWNER

        //shoot the beam!
        if (parentEntity.IsOwner)
        {
            //shooting beam in FIXEDUPDATE
            beamShooterLocal.beamShooting();
            
        }

        //SYNCHRONIZE WHEN IT'S NOT OWNER
       
        else
        {
            //draw line only
            beamShooterLocal.drawLine(playerhand.transform.forward * 10);
            
        }

        //energy sphere behavior
        beamShooterLocal.EnergySphereBehavior();
    }

}
