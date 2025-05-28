using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert; // Option to invert the look direction, if needed
    private Transform cameraTransform;

    private void Awake()
    {
        // Find the main camera in the scene
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // Make the object look at the camera
        if (cameraTransform != null)
        {
            if (invert)
            {
                Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;
                //Debug.Log("Inverted Look Direction: " + dirToCamera);
                //Debug.Log("Object Position: " + transform.position);
                transform.LookAt(transform.position + dirToCamera * -1);
            }
        }
    }
}
