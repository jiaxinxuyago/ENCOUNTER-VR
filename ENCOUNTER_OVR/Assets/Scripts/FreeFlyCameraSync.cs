using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFlyCameraSync : Bolt.EntityBehaviour<IFreeCameraState>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Attached()
    {
        base.Attached();

        //sync transform state
        state.SetTransforms(state.FreeCameraTransform, gameObject.transform);

        if (!entity.IsOwner)
        {
            gameObject.GetComponent<Camera>().enabled = false;
            gameObject.GetComponent<FreeFlyCamera>().enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
