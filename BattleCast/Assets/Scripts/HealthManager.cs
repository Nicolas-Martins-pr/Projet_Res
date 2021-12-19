using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class HealthManager : NetworkBehaviour
{
    NetworkVariable<int> damage=null;
    NetworkVariable<int> health = null;
    // Start is called before the first frame update
    void Start()
    {
        health.Value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            UpdateServer();
        }
        if (IsClient)
        {
            UpdateClient();
        }
    }

    private void UpdateClient()
    {
        
    }

    private void UpdateServer()
    {
        if (damage != null)
        {
            if (health.Value <= 0)
            {
                Destroy(gameObject);
            }
            health.Value -= damage.Value;
            damage.Value = 0;
        }
    }

    public void HealthDamage(int dam)
    {
        UpdateHealthServerRPC(dam);
    }

    [ServerRpc]
    public void UpdateHealthServerRPC(int dam)
    {
        damage.Value = dam;
       

    }
}
