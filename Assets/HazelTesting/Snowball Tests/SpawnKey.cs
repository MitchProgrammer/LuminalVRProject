using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using System;

public class SpawnKey : MonoBehaviour
{
    #region Variables :3
    public GameObject snobalObj; //snowball
    public float maxDistance = 10f; // Maximum distance to grab the balls
    public float oomph = 3f;//the strength of the throw
    public float maxBalls = 10f;

    private GameObject currentBall; // ball that is being held
    private float amountBalls;
    private bool holding;

    #endregion

    private void Start()
    {
        holding = SnowGrow.holded;
    }

    private void Update()
    {
        var primaryInput = VRDevice.Device.PrimaryInputDevice;
        if (amountBalls < maxBalls)
        {
            if (primaryInput.GetButtonDown(VRButton.One) || Input.GetMouseButtonDown(0))
            {
                if (holding)
                {
                    return;
                }
                else
                {
                    TryFourSnowBal();
                }
            }
        }
            if (primaryInput.GetButtonUp(VRButton.One) || Input.GetMouseButtonUp(0))
            {
                ReleaseBal();
            }
        
    }


    private void TryFourSnowBal()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            // Check if the object hit by the ray has the "snow" tag
            if (hit.collider.CompareTag("Snow"))
            {
                GameObject pooledObject = PoolManager.current.GetPooledObject(snobalObj.name);
                Rigidbody rbpooled = pooledObject.GetComponent<Rigidbody>();
                if (pooledObject == null)
                {
                    Debug.Log("couldn't find " + snobalObj.name + " dumbass");
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

    private void ReleaseBal()
    {
        if(currentBall != null)
        {
            currentBall.transform.SetParent(null);

            Rigidbody rb = currentBall.GetComponent<Rigidbody>();
            rb.useGravity = true;
            if(rb != null)
            {
                rb.velocity = transform.forward * oomph;
            }

            holding = false;
            currentBall = null;

        }
    }

}
