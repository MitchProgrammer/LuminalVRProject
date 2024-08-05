using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 2f;
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Update()
    {
        // Camera movement
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(horizontal, 0, vertical);

        // Camera rotation
        rotationX += Input.GetAxis("Mouse X") * lookSpeed;
        rotationY -= Input.GetAxis("Mouse Y") * lookSpeed;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
    }
}
