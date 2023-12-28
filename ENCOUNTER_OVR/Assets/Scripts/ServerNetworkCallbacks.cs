using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;


// only on the host
[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class ServerNetworkCallbacks : GlobalEventListener
{
    //player prefab to spawn from network
    public GameObject serverPlayerToSpawn;

    public Transform spawnPos;
    public override void SceneLoadLocalDone(string scene)
    {
        base.SceneLoadLocalDone(scene);

        //instanatiate server player
        if (BoltNetwork.IsServer == true)
        {
            BoltNetwork.Instantiate(serverPlayerToSpawn, spawnPos.position, Quaternion.identity);
        }
    }
}


