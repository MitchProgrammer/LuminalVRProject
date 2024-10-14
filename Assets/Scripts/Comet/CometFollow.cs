using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;

public class CometFollow : MonoBehaviour
{
    public Material trailMaterial;
    public float trailWidth = 0.1f;
    public float trailLifetime = 2f; // Duration for trail fade

    private LineRenderer currentLine;
    private List<TrailPoint> trailPoints = new List<TrailPoint>();

    public List<GameObject> trailObjects = new List<GameObject>();

    public Transform controllingObject;
    private Plane movementPlane;
    private bool hitPlane = false;

    private int trails = 0;

    private void Update()
    {
        var primaryInput = VRDevice.Device.PrimaryInputDevice;

        if (primaryInput.GetButton(VRButton.One) || Input.GetMouseButton(0))
        {
            Vector3 objectPosition = MoveComet(controllingObject);

            if (currentLine == null && hitPlane && trails <= 3)
            {
                trails++;
                CreateNewTrail();
            }
            else if (trails > 3)
            {
                Destroy(trailObjects[0]);
                trailObjects.Remove(trailObjects[0]);
                CreateNewTrail();
            }

            if (hitPlane)
            {
                currentLine.positionCount++;
                trailPoints.Add(new TrailPoint(objectPosition, Time.time)); // Track positions with spawn time
                currentLine.SetPosition(currentLine.positionCount - 1, objectPosition);
            }
            if (trailPoints.Count > 0)
            {
                UpdateTrail();
            }
        }
        else if (currentLine != null)
        {
            // Finish drawing and allow the rest of the trail to fade
            currentLine = null;
        }
        // Update the trail to fade over time
        if (trailPoints.Count > 0)
        {
            UpdateTrail();
        }
            
        else if (currentLine != null)
        {
            // Finish drawing and allow the rest of the trail to fade
            currentLine = null;
        }
    }
    Vector3 MoveComet(Transform controllingObject)
    {
        movementPlane = new Plane(Vector3.up, transform.position);
        Vector3 forwardDirection = controllingObject.forward;

        Ray ray = new Ray(controllingObject.position, forwardDirection);

        if (movementPlane.Raycast(ray, out float distance))
        {
            hitPlane = true;
            Vector3 targetPos = ray.GetPoint(distance);
            Vector3 targetPosNew = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            return targetPosNew;
        }
        else
        {
            hitPlane = false;
            return Vector3.zero;
        }
    }

    void CreateNewTrail()
    {
        GameObject trail = new GameObject("Trail");
        currentLine = trail.AddComponent<LineRenderer>();

        // Create a unique instance of the material for the trail
        Material trailInstance = new Material(trailMaterial);
        currentLine.material = trailInstance;

        currentLine.startWidth = trailWidth;
        currentLine.endWidth = trailWidth;
        currentLine.positionCount = 0;
        currentLine.useWorldSpace = true;
        trailObjects.Add(trail);
    }

    void UpdateTrail()
    {
        if (trailPoints.Count == 0) return;

        float currentTime = Time.time;

        // Update the LineRenderer positions
        currentLine.positionCount = trailPoints.Count;
        for (int i = 0; i < trailPoints.Count; i++)
        {
            currentLine.SetPosition(i, trailPoints[i].position);

            // Calculate the alpha and update the material for each point
            float age = currentTime - trailPoints[i].timeCreated;
            float normalizedAge = Mathf.Clamp01(age / trailLifetime);
            float alpha = Mathf.Lerp(1f, 0f, normalizedAge);

            // Only update the material alpha for the last point to avoid overwriting
            if (i == trailPoints.Count - 1)
            {
                UpdateMaterialAlpha(currentLine.material, alpha);
            }
            Debug.Log($"Point Index: {i}, Age: {age}, Alpha: {alpha}");
        }

        // Set the widths for the LineRenderer
        currentLine.startWidth = trailWidth; // Keep startWidth constant
        currentLine.endWidth = Mathf.Lerp(trailWidth, 0f, Mathf.Clamp01(currentTime - trailPoints[trailPoints.Count - 1].timeCreated) / trailLifetime);

        // Remove points that have exceeded their lifetime
        trailPoints.RemoveAll(p => currentTime - p.timeCreated > trailLifetime);
    }



    // Method to update the material alpha for transparency fading
    void UpdateMaterialAlpha(Material material, float alpha)
    {
        Color color = material.color;
        color.a = alpha; // Set alpha for transparency
        material.color = color; // Apply updated color with alpha to the material
    }

    // Helper class to store position and time created
    private class TrailPoint
    {
        public Vector3 position;
        public float timeCreated;

        public TrailPoint(Vector3 position, float timeCreated)
        {
            this.position = position;
            this.timeCreated = timeCreated;
        }
    }
}