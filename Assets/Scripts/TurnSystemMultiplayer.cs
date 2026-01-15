using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Netcode;

public class TurnSystemMultiplayer : TurnSystem
{
    public override void OnNetworkSpawn()
    {
        turnNumber.OnValueChanged += (int oldValue, int newValue) =>
        {
            if (!NetworkManager.IsServer)
            {
                isPlayerTurn = !isPlayerTurn;
                InvokeOnTurnChange();
            }
        };

        if (NetworkManager.IsHost)
        {
            isPlayerTurn = false;
        }
        else
        {
            isPlayerTurn = true;
        }

        InvokeOnTurnChange();

    }

    public override void NextTurn()
    {
        NextTurnServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    private void NextTurnServerRPC()
    {
        base.NextTurn();
    }
}
