using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using System;

public class CometFollow : MonoBehaviour
{
    public Transform controllingObject;
    public LineRenderer lineRenderer;

    private bool isDragging;
    private Plane movementPlane;
    private List<Vector3> points = new List<Vector3>();
    private List<Color> pointColours = new List<Color>();
    public int maxPoints = 100;
    public float fadeDuration = 1f;
    public Color initialColour = Color.white;
    public int timer;

    private void Start()
    {
        lineRenderer.positionCount = 0;
    }

    private void Update()
    {
        var primaryInput = VRDevice.Device.PrimaryInputDevice;

        if (primaryInput.GetButtonDown(VRButton.One) || Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            isDragging = true;
            points.Clear();
            pointColours.Clear();
            lineRenderer.positionCount = 0;
        }

        if (primaryInput.GetButtonUp(VRButton.One) || Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            lineRenderer.positionCount = points.Count;
        }

        if (isDragging)
        {
            MoveComet(controllingObject);
            AddPointToLine();
        }
    }

    void MoveComet(Transform controllingObject)
    {
        movementPlane = new Plane(Vector3.up, transform.position);
        Vector3 forwardDirection = controllingObject.forward;

        Ray ray = new Ray(controllingObject.position, forwardDirection);

        if (movementPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPos = ray.GetPoint(distance);
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        }
    }

    void AddPointToLine()
    {
        Debug.Log("Initial Colour: " + initialColour);
        points.Add(transform.position);
        pointColours.Add(initialColour);
        
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
        lineRenderer.startColor = initialColour;
        lineRenderer.endColor = initialColour;

        if (points.Count > maxPoints)
        {
            StartCoroutine(FadePoint(0));
        }
    }

    System.Collections.IEnumerator FadePoint(int index)
    {
        float elapsed = 0f;
        Color startColour = pointColours[index];
        Color endColour = new Color(startColour.r, startColour.g, startColour.b, 0);


        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;

            pointColours[index] = Color.Lerp(startColour, endColour, t);
            UpdateLineRenderer();

            elapsed += Time.deltaTime;
            yield return null;
        }

        points.RemoveAt(index);
        pointColours.RemoveAt(index);
        UpdateLineRenderer();
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.startColor = pointColours[i];
            lineRenderer.endColor = pointColours[i];
        }
    }
}
