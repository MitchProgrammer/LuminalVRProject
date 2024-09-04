using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float distanceFromCamera = 2.0f;

    void Update()
    {
        // Calculate the position in front of the camera
        Vector3 newPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;

        // Update the canvas position
        transform.position = newPosition;

        // Make the canvas face the camera
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }
}
