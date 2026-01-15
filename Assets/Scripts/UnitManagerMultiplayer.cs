using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class UnitManagerMultiplayer : UnitManager
{
    public override void OnNetworkSpawn()
    {
        if (NetworkManager.IsHost)
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
