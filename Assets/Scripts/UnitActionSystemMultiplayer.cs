using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystemMultiplayer : UnitActionSystem
{
    protected override void HandleSelectedAction()
    {
        if (selectedUnit == null)
            return; ;

        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
        if (Input.GetMouseButtonDown(0))
        {
            HandleSelectedActionServerRPC(selectedUnit.GetComponent<NetworkObject>(), selectedAction.GetActionType(), mouseGridPosition);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void HandleSelectedActionServerRPC(NetworkObjectReference selectedUnit, ActionType actionType, GridPosition gridPosition)
    {
        Debug.Log("HandleSelectedActionServerRPC");

        if (!selectedUnit.TryGet(out NetworkObject target) || target.GetComponent<Unit>() == null)
        {
            Debug.LogError("References network object don't exist at client side");
            return;
        }
        this.selectedUnit = target.GetComponent<Unit>();

        BaseAction action = GetActionFromUnit(target, actionType);
        if (action == null)
        {
            Debug.LogError("Action does not exist on unit");
            return;
        }
        this.selectedAction = action;

        SelectedActionLogic(gridPosition);
    }

    private BaseAction GetActionFromUnit(NetworkObject unit, ActionType actionType)
    {
        BaseAction ret;
        switch (actionType)
        {
            case ActionType.Move:
                ret = unit.GetComponent<MoveAction>();
                break;
            case ActionType.Shoot:
                ret = unit.GetComponent<ShootAction>();
                break;
            case ActionType.Gernade:
                ret = unit.GetComponent<GrenadeAction>();
                break;
            case ActionType.Interact:
                ret = unit.GetComponent<InteractAction>();
                break;
            case ActionType.Sword:
                ret = unit.GetComponent<SwordAction>();
                break;
            case ActionType.Spin:
                ret = unit.GetComponent<SpinAction>();
                break;
            default:
                ret = null;
                break;
        }

        return ret;
    }
}