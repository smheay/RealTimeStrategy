using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : NetworkBehaviour
{

    [SerializeField] private Health health = null;


    ///  Every time a base is spawned it adds to a list if there is 1 left then that player wins
    // Whenever a base is spawned and despawned raise these events 
    // THe gameOVerHandler is listening to these 
    public static event Action<UnitBase> ServerOnBaseSpwaned;
    public static event Action<UnitBase> ServerOnBaseDespwaned;


    #region Server

    public override void OnStartServer()
    {

        health.ServerOnDie += ServerHandleDie;
        ServerOnBaseSpwaned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnBaseDespwaned?.Invoke(this);
        health.ServerOnDie -= ServerHandleDie;

    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }


    #endregion


    #region Client



    #endregion 


}
