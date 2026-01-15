using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class HealthSystem : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> health = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private int healthMax;
    public event EventHandler OnDead;
    public event EventHandler OnDameged;

    private void Awake()
    {
        healthMax = health.Value; // Store the initial max health
    }

    public override void OnNetworkSpawn()
    {
        health.OnValueChanged += (int prevValue, int newValue) =>
        {
            if (!NetworkManager.IsServer)
            {
                OnDameged?.Invoke(this, EventArgs.Empty); // Notify subscribers that damage has occurred
            }
        };
    }

    public void Damage(int damageAmount)
    {
        health.Value -= damageAmount;
        if (health.Value < 0)
        {
            health.Value = 0;
        }

        OnDameged?.Invoke(this, EventArgs.Empty); // Notify subscribers that damage has occurred

        if (health.Value == 0)
        {
            Die();
        }

        Debug.Log("Health left: " + health);
    }

    public void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
        OnDeadClientRPC();
    }

    [ClientRpc]
    private void OnDeadClientRPC()
    {
        Debug.Log("OnDeadClientRPC 1");
        if (NetworkManager.IsServer) return;
        Debug.Log("OnDeadClientRPC 2");
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health.Value / healthMax; // Return normalized health value
    }
}
