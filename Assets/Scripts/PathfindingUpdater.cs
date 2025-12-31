using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        //GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        DestructibleCrate.OnAnyDestructibleCrateDestroyed += DestructibleCrate_OnAnyDestructibleCrateDestroyed;
    }

    private void DestructibleCrate_OnAnyDestructibleCrateDestroyed(object sender, System.EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;  
        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }
}
