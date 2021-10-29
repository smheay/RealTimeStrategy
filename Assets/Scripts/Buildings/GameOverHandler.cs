using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverHandler : NetworkBehaviour
{



    private List<UnitBase> bases = new List<UnitBase>();

    //public static int ServerOnGameOver { get; internal set; }
    public static event Action ServerOnGameOver;



    public static event Action<string> ClientOnGameOver;



    

    #region Server


    public override void OnStartServer()
    {
        UnitBase.ServerOnBaseSpwaned += ServerHandleBaseSpawned;    // on base spawned add it to list
        UnitBase.ServerOnBaseDespwaned += ServerHandleBaseDespawned;   // on base despawned add it to list
    }


    public override void OnStopServer()
    {
        UnitBase.ServerOnBaseSpwaned -= ServerHandleBaseSpawned;
        UnitBase.ServerOnBaseDespwaned -= ServerHandleBaseDespawned;
    }


    [Server]
    private void ServerHandleBaseSpawned(UnitBase unitBase)
    {
        bases.Add(unitBase);
    }


    [Server]
    private void ServerHandleBaseDespawned(UnitBase unitBase)
    {


        bases.Remove(unitBase);

        if (bases.Count != 1) { return; }

        int playerId = bases[0].connectionToClient.connectionId;

        RpcGameOver($"Player {playerId}");


        ServerOnGameOver?.Invoke();
    }

    #endregion


    #region Client
    //The server tells the client the game is over with the Rpc
    [ClientRpc]
    private void RpcGameOver(string winner)
    {
        ClientOnGameOver?.Invoke(winner);
    }

    #endregion 




}
