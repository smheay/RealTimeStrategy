using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

   

    [SyncVar (hook = nameof(HandleHealthUpdated))]
    private int currentHealth;

    public event Action <int, int> ClientOnHealthUpdated;



    //Raises action on death
    public event Action ServerOnDie;

    public override void OnStartServer()
    {
        currentHealth = maxHealth;
    }


    #region Server

    [Server]
    public void DealDamage(int damageAmount)
    {
        if (currentHealth == 0) { return; }

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

        if (currentHealth != 0) { return; }

        //dead after this 
        
       
        ServerOnDie?.Invoke(); //raise an event that raises when something dies

    }

    #endregion


    #region Client


    private void HandleHealthUpdated(int oldHealth, int newHealth )
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }



    #endregion



}
