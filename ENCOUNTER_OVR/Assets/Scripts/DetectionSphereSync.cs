using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSphereSync : Bolt.EntityBehaviour<IMainPlayerState>
{
    private string playerTag;
    private string otherPlayerTag;

    private bool otherPlayerInRange = false;
    private bool otherPlayerIsColliding = false;

    private void Awake()
    {
        //check player tag and know the other player tag
        playerTag = transform.parent.gameObject.tag;

        if (playerTag == "Player_Server")
        {
            otherPlayerTag = "Player_Client";
        }
        else
        {
            otherPlayerTag = "Player_Server";
        }

    }

    void Start()
    {
        
    }

    public override void Attached()
    {
        base.Attached();

        //sync transform state
        state.SetTransforms(state.DetectionSphereTransform, gameObject.transform);

    }
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == otherPlayerTag)
        {
            otherPlayerInRange = true;
            otherPlayerIsColliding = true;
        }

        if (other.gameObject.tag == "Plant" && otherPlayerInRange)
        {
            other.gameObject.SendMessage("PlayerCloseRespond_On");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == otherPlayerTag)
        {
            otherPlayerInRange = false;
        }

        if (other.gameObject.tag == "Plant")
        {
            other.gameObject.SendMessage("PlayerCloseRespond_Off");
            otherPlayerIsColliding = false;
        }
    }
}
