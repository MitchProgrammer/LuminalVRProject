using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]

public class SnowGrow : MonoBehaviour
{
    public Transform transfirm; 
    public float size = 1;
    public Rigidbody rigidBody; 
    public Vector3 velotown; //velocity of the growing obj
    public bool isGrowTime; //if it is on the growing material
    public float growthRate = 0.1f; 
    static public bool holded; //if the snowball isn't being held

    private void Start()
    {
        transfirm = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        velotown = rigidBody.velocity;
        if (isGrowTime && !holded)
        {
            float velocityMagnitude = velotown.magnitude; // Get the magnitude of the velocity
            if (velocityMagnitude > 0)
            {
                // Adjust growth rate based on current size to slow growth as size increases
                float growthAmount = velocityMagnitude * growthRate / size * Time.deltaTime;
                size += growthAmount;
                transform.localScale = new Vector3(size, size, size); // Apply new size
            }
        }
    }
    #region touchytime
    //is touching ground
    private void OnCollisionStay(Collision touchy)
    {
        if (touchy.collider.tag == "Snow" && !holded)
        {
            isGrowTime = true;
        }
    }
    private void OnCollisionExit(Collision noTouchy)
    {
        if(noTouchy.collider.tag == "Snow" && !holded)
        {
            isGrowTime = false;
        }
    }
    #endregion

}
