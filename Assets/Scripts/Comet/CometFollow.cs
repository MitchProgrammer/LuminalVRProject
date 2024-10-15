using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;

public class CometFollow : MonoBehaviour
{
    public Transform controllingObject;

    private bool isDragging;
    private Plane movementPlane;
    private Vector3 forwardDirection;

    private void Update()
    {
        var primaryInput = VRDevice.Device.PrimaryInputDevice;

        if (primaryInput.GetButtonDown(VRButton.One) || Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        if (primaryInput.GetButtonUp(VRButton.One) || Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            MoveComet(controllingObject);
        }
    }

    void MoveComet(Transform controllingObject)
    {
        movementPlane = new Plane(Vector3.up, transform.position);
        forwardDirection = controllingObject.forward;

        Ray ray = new Ray(controllingObject.position, forwardDirection);

        if (movementPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPos = ray.GetPoint(distance);
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        }
    }
}