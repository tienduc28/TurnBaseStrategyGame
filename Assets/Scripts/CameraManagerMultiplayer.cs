using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraManagerMultiplayer : CameraManager
{
    protected override void HandleActionCamera(bool cameraOn, Vector3 position, Vector3 lookAt)
    {
        base.HandleActionCamera(cameraOn, position, lookAt);

        HandleActioncameraClientRPC(cameraOn, position, lookAt);
    }

    [ClientRpc]
    private void HandleActioncameraClientRPC(bool cameraOn, Vector3 position, Vector3 lookAt)
    {
        base.HandleActionCamera(cameraOn, position, lookAt);
    }
}
