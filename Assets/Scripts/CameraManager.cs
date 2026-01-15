using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraManager : NetworkBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCamera();
    }
    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }   

    protected virtual void HandleActionCamera(bool cameraOn, Vector3 position, Vector3 lookAt)
    {
        if (cameraOn)
        {
            ShowActionCamera();
            actionCameraGameObject.transform.position = position;
            actionCameraGameObject.transform.LookAt(lookAt);
        }
        else
        {
            HideActionCamera();
        }
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;

                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;

                Vector3 actionCameraPosition = 
                    shooterUnit.GetWorldPosition() + 
                    cameraCharacterHeight + 
                    shoulderOffset +
                    (shootDir * -1f);

                HandleActionCamera(true, actionCameraPosition, targetUnit.GetWorldPosition() + cameraCharacterHeight);
                break;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e) 
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HandleActionCamera(false, Vector3.zero, Vector3.zero);
                break;
        }
    }
}
