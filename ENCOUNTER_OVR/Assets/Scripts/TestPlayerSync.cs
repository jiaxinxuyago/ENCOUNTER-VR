using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerSync : Bolt.EntityBehaviour<ITestPlayerState>
{
    public override void Attached()
    {
        base.Attached();

        //sync player transform data

         state.SetTransforms(state.PlayerTransform, gameObject.transform);

    }

    public override void SimulateOwner()
    {
        base.SimulateOwner();
    }

    
}
