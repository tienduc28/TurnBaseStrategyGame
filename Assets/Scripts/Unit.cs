using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class Unit : NetworkBehaviour
{
    private const int MAX_ACTION_POINTS = 5;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;

    [SerializeField]
    private NetworkVariable<int> actionPoints = new NetworkVariable<int>(MAX_ACTION_POINTS, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField]
    private bool isEnemy;

    private void Awake()
    {
        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }

    public override void OnNetworkSpawn()
    {
        actionPoints.OnValueChanged += (int prevValue, int newValue) =>
        {
            if (!NetworkManager.IsServer)
                OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            //Unit changed Grid Position
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointCost());
            //Debug.Log(actionPoints);
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoints.Value >= baseAction.GetActionPointCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int mount)
    {
        actionPoints.Value -= mount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoints.Value;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        //This is bad scripting, should seperate OnTurnChanged into different callbacks
        if (NetworkManager != null && NetworkManager.IsClient && !NetworkManager.IsServer)
        {
            return;
        }

        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy()) && TurnSystem.Instance.IsPlayerTurn())
        {
            actionPoints.Value = MAX_ACTION_POINTS;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void SetUnitAsEnemy()
    {
        isEnemy = true;
    }

    public void SetUnitAsAlly()
    {
        isEnemy = false;
    }

    public void Damage(int damageAmount)
    {
        Debug.Log(transform + "damaged");
        healthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);

        //Delay despawn till next frame so multiplayer logic work correctly
        StartCoroutine(DelayDespawn());
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }   

    IEnumerator DelayDespawn()
    {
        //This is bad scripting, should seperate OnTurnChanged into different callbacks
        if (NetworkManager != null && NetworkManager.IsClient && !NetworkManager.IsServer)
        {
            yield break;
        }

        yield return null;
        if (NetworkObject != null && NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}