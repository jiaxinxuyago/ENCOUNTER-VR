using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyChargerBehaviorSync : Bolt.EntityBehaviour<IEnergyChargerState>
{
    private Light mylight;
    private Animation myAnimation;

    //life span
    private float timer;
    public float lifeSpan;

    //decay
    private Vector3 scaleFactor;

    //grab
    private bool isGrabbed;
    void Awake()
    {
        mylight = GetComponent<Light>();
        myAnimation = GetComponent<Animation>();

        timer = 0.0f;
        lifeSpan = 60.0f;

        scaleFactor = new Vector3(0.0005f, 0.0005f, 0.0005f);
    }

    public override void Attached()
    {
        base.Attached();

        //sync transform state
        state.SetTransforms(state.ChargerTransform, gameObject.transform);
        
    }

    void Update()
    {
       timer += Time.deltaTime;

        if (lifeSpan < timer)
        {
            ChargerDying();
        }

        if (isGrabbed)
        {
            ChargerDying();
        }
    }

    public void ChargerDying()
    {
        if (myAnimation.isPlaying)
        {
            myAnimation.Stop();
        }

        mylight.range -= 0.002f;
        mylight.intensity -= 0.004f;
        transform.localScale -= scaleFactor;

        if (transform.localScale.y < 0.002f)
        {
            BoltNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hand")
        {
            other.gameObject.transform.parent.transform.parent.transform.parent.gameObject.SendMessage("CheckIsGrabbing", gameObject);
            other.gameObject.transform.parent.transform.parent.transform.parent.gameObject.SendMessage("RechargeEnergySphere");
        }
    }

    public void GrabbingDecay()
    {
        isGrabbed = true;
    }
}
