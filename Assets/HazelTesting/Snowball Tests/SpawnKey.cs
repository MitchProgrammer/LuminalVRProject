using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using System;

public class SpawnKey : MonoBehaviour
{
    #region Variables :3
    public GameObject snowballObj; //snowball
    public float maxDistance = 10f; // Maximum distance to grab the balls
    public float throwStrength = 3f;//the strength of the throw
    public float maxBalls = 10f; // maximum amount of snowballs allowed at one time

    private GameObject currentBall; // ball that is being held
    public float amountBalls; //current amount of balls at one time
    public bool holding; //if the ball is being held

    #endregion

    private void Update()
    {
        var primaryInput = VRDevice.Device.PrimaryInputDevice;
        if (primaryInput.GetButtonDown(VRButton.One) || Input.GetMouseButtonDown(0))
        {
            if (holding)
            {
            return;
            }
            else
            {
                TryForSnowBall();
            }
        }
        if (primaryInput.GetButtonUp(VRButton.One) || Input.GetMouseButtonUp(0))
        {
            ReleaseBall();
        }
        
    }


    private void TryForSnowBall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            //check if the object hit by the ray has the snowball tag
            if(hit.collider.CompareTag("Snowball"))
            {
                GameObject existingObject = hit.transform.gameObject;
                Rigidbody rbExistingObj = existingObject.GetComponent<Rigidbody>();
                if(existingObject == null)
                {
                    Debug.Log("what? there's no snowball??");
                    return;
                }
                rbExistingObj.velocity = Vector3.zero;
                existingObject.transform.SetParent(transform);
                existingObject.transform.localPosition = Vector3.zero;
                existingObject.transform.localRotation = Quaternion.identity;
                rbExistingObj.useGravity = false;

                currentBall = existingObject;
                holding = true;
            }
            // Check if the object hit by the ray has the snow tag and that the current amount of balls in the world doesn't exceed the maxiumum
            else if (hit.collider.CompareTag("Snow") && amountBalls < maxBalls)
            {
                GameObject pooledObject = PoolManager.current.GetPooledObject(snowballObj.name);
                Rigidbody rbpooled = pooledObject.GetComponent<Rigidbody>();
                if (pooledObject == null)
                {
                    Debug.Log("couldn't find " + snowballObj.name + " hmmm");
                    return;
                }
                pooledObject.transform.SetParent(transform);
                pooledObject.transform.localPosition = Vector3.zero;
                pooledObject.transform.localRotation = Quaternion.identity;
                rbpooled.useGravity = false;
                amountBalls++;
                
                
                pooledObject.SetActive(true);
                currentBall = pooledObject;
                holding = true;

            }
        }
    }

    private void ReleaseBall()
    {
        if(currentBall != null)
        {
            currentBall.transform.SetParent(null);

            Rigidbody rb = currentBall.GetComponent<Rigidbody>();
            rb.useGravity = true;
            if(rb != null)
            {
                rb.velocity = transform.forward * throwStrength; //throws the snowball
            }
            //resets the hand's currently held ball and if it's holding a ball
            holding = false;
            currentBall = null;

        }
    }

}
