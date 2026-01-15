using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameVFXManagerMultiplayer : GameVFXManager
{


    public override Transform SpawnBulletProjectile(Vector3 shootPosition, Vector3 targetUnitShootAtPosition)
    {
        Transform ret = base.SpawnBulletProjectile(shootPosition, targetUnitShootAtPosition);

        SpawnBulletProjectileClientRPC(shootPosition, targetUnitShootAtPosition);

        return ret;
    }

    [ClientRpc]
    private void SpawnBulletProjectileClientRPC(Vector3 shootPosition, Vector3 targetUnitShootAtPosition)
    {
        if (NetworkManager.IsServer) return;
        base.SpawnBulletProjectile(shootPosition, targetUnitShootAtPosition);
    }

    public override Transform SpawnHitVFX(Vector3 position)
    {
        Transform ret = base.SpawnHitVFX(position);

        SpawnHitVFXClientRPC(position);

        return ret;
    }

    [ClientRpc]
    private void SpawnHitVFXClientRPC(Vector3 position)
    {
        base.SpawnHitVFX(position);
    }
}
