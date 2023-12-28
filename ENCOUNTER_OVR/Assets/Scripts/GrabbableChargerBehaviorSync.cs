using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableChargerBehaviorSync : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        /*if (other.gameObject.tag == "Player_Client" || other.gameObject.tag == "Player_Server")
        {
            if (other.gameObject.GetComponent<FPCLocalBehavior>() != null)
            {
                transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                
                if (other.gameObject.GetComponent<MainPlayerSync>().playerLocalBehavior.isGrabbingCharger_FPC)
                {
                    transform.parent.gameObject.GetComponent<EnergyChargerBehaviorSync>().isGrabbed = true;
                }
            }
        }*/
    }
}
