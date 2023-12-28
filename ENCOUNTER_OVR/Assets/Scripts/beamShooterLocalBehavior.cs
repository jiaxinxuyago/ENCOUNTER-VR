using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beamShooterLocalBehavior : MonoBehaviour
{
    //find target object to parent to
    public GameObject player;
    public GameObject playerHand;
    private GameObject beamEnergySphere;
    private FPCLocalBehavior FPCPlayerLocalBehavior;
    private OVRLocalBehavior OVRPlayerLocalBehavior;

    //define raycast
    [HideInInspector] public RaycastHit hit;

    //input state
    [HideInInspector] public bool isShooting_FPC;
    [HideInInspector] public bool isShooting_OVR;

    //get line renderer
    private LineRenderer lineRenderer;
    private MeshRenderer beamEnergySphereRenderer;

    // energy depletion
    private float currentWidth = 0.4f;
    private float depeletingRate = 0.003f;
    public bool isDepeleted;
    public bool isHittingSeed;
    public bool isRecharging;

    // Start is called before the first frame update
    void Start()
    {
        //differentiate FPC vs OVR

        if (player.GetComponent<FPCLocalBehavior>() != null && player.GetComponent<OVRLocalBehavior>() == null)
        {
            FPCPlayerLocalBehavior = player.GetComponent<FPCLocalBehavior>();
        }
        else if (player.GetComponent<FPCLocalBehavior>() == null && player.GetComponent<OVRLocalBehavior>() != null)
        {
            OVRPlayerLocalBehavior = player.GetComponent<OVRLocalBehavior>();
        }
        
        lineRenderer = GetComponent<LineRenderer>();
        beamEnergySphere = gameObject.transform.GetChild(0).gameObject;
        beamEnergySphereRenderer = beamEnergySphere.GetComponent<MeshRenderer>();

        lineRenderer.enabled = false;
        beamEnergySphereRenderer.enabled = false;
        
    }

   
    //raycast
    void raycastOut()
    {
        //set layer mask
        int layerMask = ~(1 << 8);

        //draw raycast in debug mode
        Transform raycastFrom = playerHand.transform;

        if (Physics.Raycast(raycastFrom.position, raycastFrom.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(raycastFrom.position, raycastFrom.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(raycastFrom.position, raycastFrom.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }

        
    }


    //shoot out beam
    public void beamShooting()
    {
        if (FPCPlayerLocalBehavior != null)
        {
            if (FPCPlayerLocalBehavior.isShooting_FPC)
            {
                if (!isDepeleted)
                {
                    raycastOut();
                    showBeam();
                    raycastSendMessages();

                    if (hit.collider != null)
                    {
                        if (hit.transform.gameObject.tag == "Seed")
                        {
                            isHittingSeed = true;
                        }
                        else
                        {
                            isHittingSeed = false;
                        }

                    }

                }
                else
                {
                    lineRenderer.enabled = false;
                }
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }else if(OVRPlayerLocalBehavior != null)
        {
            if (OVRPlayerLocalBehavior.isShooting_OVR)
            {
                if (!isDepeleted)
                {
                    raycastOut();
                    showBeam();
                    raycastSendMessages();

                    if (hit.collider != null)
                    {
                        if (hit.transform.gameObject.tag == "Seed")
                        {
                            isHittingSeed = true;
                        }
                        else
                        {
                            isHittingSeed = false;
                        }

                    }

                }
                else
                {
                    lineRenderer.enabled = false;
                }
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
     
    }

    //draw a line
    public void showBeam()
    {

        if (hit.collider != null)
        {
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }

        //draw line for beam
        drawLine(hit.point);


    }

    public void drawLine(Vector3 secondPos)
    {
        if (lineRenderer.enabled)
        {
            //start drawing line
            Vector3 startPos = playerHand.transform.position;
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.startWidth = currentWidth;
            lineRenderer.endWidth = 0.02f;
            
            //form line to a beamlength away from the hitting target on z
            lineRenderer.SetPosition(1, secondPos);

        }

    }

    //RAYCAST SEND MESSAGES

    public void raycastSendMessages()
    {
        if (hit.collider != null) {

            //Find hot object tag
            GameObject targetObject = hit.collider.gameObject;
            string targetTag = targetObject.tag;

            //For Slots
            if (targetTag == "Slot")
            {
                //Report to owner: SLOT ID; WHERE IS THE HIT
                player.SendMessage("CastedSlotID", targetObject.GetComponent<slotGenerationSync>().mySlotID);
                player.SendMessage("CurrentHittingPosition", hit.point);
            }

            if (targetTag == "Seed")
            {
                //Report to owner: SLOT ID; WHERE IS THE HIT
                player.SendMessage("CastedSeedInfo", targetObject.GetComponent<SeedsGenerationSync>());
            }

        }

    }

    //Energy beam states
    public void EnergySphereBehavior()
    {
        //move beam sphere to hand
        beamEnergySphere.transform.position = playerHand.transform.position;
        
        //Show energy sphere when it's not depleted
        if (!isDepeleted)
        {
            beamEnergySphereRenderer.enabled = true;
            
        }
        else
        {
            beamEnergySphereRenderer.enabled = false;
        }

        //Start depletion when light is beamed
        if (lineRenderer.enabled)
        {
           EnergyDeleption();
        }

    }

    public void EnergyDeleption()
    {
        if (isHittingSeed)
        {
            if (beamEnergySphere.transform.localScale.x > 0.1)
            {
                if (currentWidth > 0.02f)
                {
                    currentWidth -= depeletingRate / 2;
                }
                beamEnergySphere.transform.localScale -= new Vector3(depeletingRate, depeletingRate, depeletingRate);
            }
            else
            {
                isDepeleted = true;
            }
        }
            
    }

    public void Recharge()
    {
        isRecharging = true;
    }

    public void Update()
    {
        if (isRecharging)
        {
            if (beamEnergySphere.transform.localScale.x < 0.5)
            {
                beamEnergySphere.transform.localScale += new Vector3(depeletingRate * 5, depeletingRate * 5, depeletingRate * 5);

                if (beamEnergySphere.transform.localScale.x > 0.5)
                {
                    beamEnergySphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    isRecharging = false;
                }
            }
           
            if (currentWidth < 0.4)
            {
                currentWidth += depeletingRate * 15;
                if (currentWidth > 0.4)
                {
                    currentWidth = 0.4f;
                }
            }
        }
    }

}
