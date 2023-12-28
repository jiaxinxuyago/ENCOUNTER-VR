using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;


// only on join
[BoltGlobalBehaviour(BoltNetworkModes.Client)]
public class ClientNetworkCallbacks : GlobalEventListener
{
    //player prefab to spawn from network
    public GameObject clientPlayerToSpawn;

    public  Transform spawnPos;
    public override void SceneLoadLocalDone(string scene)
    {
        base.SceneLoadLocalDone(scene);

        
        //instanatiate client player
        if (BoltNetwork.IsClient == true)
        {
            BoltNetwork.Instantiate(clientPlayerToSpawn, spawnPos.position, Quaternion.identity);
        }
    }
}