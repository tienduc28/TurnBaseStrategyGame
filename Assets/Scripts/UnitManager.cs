using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitManager : NetworkBehaviour
{
    public static UnitManager Instance { get; private set; }

    protected List<Unit> unitList;
    protected List<Unit> friendlyUnitList;
    protected List<Unit> enemyUnitList;

    public event EventHandler OnGameResult;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one UnitManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    protected void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, System.EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Add(unit);
        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            friendlyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, System.EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Remove(unit);
        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendlyUnitList.Remove(unit);
        }

        if (enemyUnitList.Count == 0)
        {
            OnGameEnd(true);
        }
        else if (friendlyUnitList.Count == 0)
        {
            OnGameEnd(false);
        }

    }
    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }
    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }

    protected virtual void OnGameEnd(bool isWin)
    {
        OnGameResult?.Invoke(this, new GameEndArgs(isWin));
    }

}

public class GameEndArgs : EventArgs
{
    public bool IsWin { get; private set; }

    public GameEndArgs(bool isWin)
    {
        IsWin = isWin;
    }
}