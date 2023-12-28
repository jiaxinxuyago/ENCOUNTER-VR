using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;


//Manage callback events in the scene
public class CallbackManager : Bolt.EntityBehaviour<IMainPlayerState>
{
    private string playerTag;
    private string otherPlayerTag;

    private void Awake()
    {
        //check player tag and know the other player tag
        playerTag = gameObject.tag;

        if (playerTag == "Player_Server")  
        {
            otherPlayerTag = "Player_Client";
        }
        else
        {
            otherPlayerTag = "Player_Server";
        }

    }
    public override void Attached()
    {
        base.Attached();

        //CALLBACK FUNCTIONS

        //SYNC casted slot
        state.AddCallback("SlotIDToGrow", SeedGrowCommand);
        state.AddCallback("PlantIDToGrow", SeedGrowCommand);
        state.AddCallback("SeedIsOccupied", SeedGrowCommand);
    }


    void Update()
    {
        SeedGrowCommand();
    }

    //CALLBACK FUNCTIONS

    //initialize current slot ID;
    private int currentSlotID = 999;

    //Update slot number
    public void CastedSlotID(int slotID)
    {
        currentSlotID = slotID;
    }

    //initialize hitting point
    private Vector3 beamHittingPos;

    //update hitting point
    public void CurrentHittingPosition(Vector3 currentPos)
    {
        beamHittingPos = currentPos;
    }

    //initialize seeds double ID holder
    private int thisSeedSlotID = 999;
    private int thisSeedPlantID = 999;
    private bool thisSeedisOccupied = false;

    //update seeds info
    public void CastedSeedInfo(SeedsGenerationSync thisSeed)
    {
        thisSeedSlotID = thisSeed.slotID;
        thisSeedPlantID = thisSeed.plantID;
        thisSeedisOccupied = thisSeed.occupied;
    }


    //initialize slot and seed list from the scene
    public GameObject[] seedsArr;

    //send message to grow this seed
    private void SeedGrowCommand()
    {
        //sync from owner to not owner

        //sync slot 
        if (entity.IsOwner)
        {
            state.SlotIDToGrow = thisSeedSlotID;
            state.PlantIDToGrow = thisSeedPlantID;
            state.SeedIsOccupied = thisSeedisOccupied;
        }
        else
        {
            thisSeedSlotID = state.SlotIDToGrow;
            thisSeedPlantID = state.PlantIDToGrow;
        }


        seedsArr = GameObject.FindGameObjectsWithTag("Seed");

        //send grow seed message
        foreach (GameObject seed in seedsArr)
        {
            if (seed.GetComponent<SeedsGenerationSync>().slotID == state.SlotIDToGrow && seed.GetComponent<SeedsGenerationSync>().plantID == state.PlantIDToGrow)
            {

                if (entity.IsOwner && state.IsBeaming && !state.SeedIsOccupied)
                {
                    seed.SendMessage("GrowSeed");
                }
                else
                {
                    seed.GetComponent<SeedsGenerationSync>().occupied = state.SeedIsOccupied;
                }
            }
        }

    }

    public void SelfDestroyCommand(GameObject thisPlant)
    {
        if (thisPlant != null)
        {
            if (entity.IsOwner)
            {
                BoltNetwork.Destroy(thisPlant);
            }
        }
    }
}
