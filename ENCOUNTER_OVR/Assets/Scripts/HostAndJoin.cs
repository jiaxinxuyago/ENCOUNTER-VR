using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Bolt.Matchmaking;
using UdpKit;
using System;

public class HostAndJoin : GlobalEventListener
{
    //Initialzie Session ID and Scene to Load
    public string mySessionID;
    public string mySceneToLoad;


    //Call from Host Game Button
    public void startServer()
    {
        BoltLauncher.StartServer();
    }

    //Call from Join Game Button
    public void startClient()
    {
        BoltLauncher.StartClient();
    }

    public override void BoltStartDone()
    {
        BoltMatchmaking.CreateSession(sessionID: mySessionID, sceneToLoad: mySceneToLoad);
    }

    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        base.SessionListUpdated(sessionList);

         foreach (var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;

            if (photonSession.Source == UdpSessionSource.Photon)
            {
                BoltMatchmaking.JoinSession(photonSession);
            }
        }
    }
}
