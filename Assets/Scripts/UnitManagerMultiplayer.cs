using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class UnitManagerMultiplayer : UnitManager
{
    bool networkSpawned = false;
    bool inited = false;
    public override void OnNetworkSpawn()
    {
        networkSpawned = true;   
    }

    // Initialization should not be here
    // Should change unit initialization to be handled completely in UnitManager then put this with flag in Start and OnNetworkSpawn
    private void Update()
    {
        if (networkSpawned && !inited)
        {
            InitMultiplayer();
            inited = true;
        }
    }

    private void InitMultiplayer()
    {
        if (NetworkManager.IsServer)
        {
            //Switch size for host
            List<Unit> temp = friendlyUnitList.ToList();
            friendlyUnitList = enemyUnitList.ToList();
            enemyUnitList = temp.ToList();

            foreach (var unit in friendlyUnitList)
            {
                unit.SetUnitAsAlly();
            }

            foreach (var unit in enemyUnitList)
            {
                unit.SetUnitAsEnemy();
            }
        }
        else
        {
            return;
        }
    }
}
