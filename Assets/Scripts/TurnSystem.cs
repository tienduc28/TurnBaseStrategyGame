using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Netcode;

public class TurnSystem : NetworkBehaviour
{
    public static TurnSystem Instance { get; private set; } // Singleton instance

    public event EventHandler OnTurnChanged;

    protected NetworkVariable<int> turnNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    protected bool isPlayerTurn = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one TurnSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log("Turn: " + turnNumber);
    }

    public virtual void NextTurn()
    {
        turnNumber.Value++;
        Debug.Log("Is Player Turn: " + isPlayerTurn);
        isPlayerTurn = !isPlayerTurn;
        Debug.Log("Is Player Turn: " + isPlayerTurn);
        Debug.Log("Turn: " + turnNumber);

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    protected void InvokeOnTurnChange()
    {
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber.Value;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
