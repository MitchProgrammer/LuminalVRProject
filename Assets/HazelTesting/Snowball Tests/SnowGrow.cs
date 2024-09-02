using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]

public class SnowGrow : MonoBehaviour
{
    public Transform transfirm;
    public Rigidbody rigidBody;

    public float size = 0.1f;
    public float maxSize = 2f;
    
    public Vector3 velotown; //velocity of the growing obj
    public bool isGrowTime; //if it is on the growing material
    public float growthRate = 0.1f; 
    public bool holded; //if the snowball isn't being held

    public float shrinkTime = 20f;
    private bool shrink = false;

    public SpawnKey handSpawnKey;

    private void Start()
    {
        transfirm = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        handSpawnKey = GetComponentInParent<SpawnKey>();
    }

    private void Update()
    {
        holded = handSpawnKey.holding;
        velotown = rigidBody.velocity;
        if (isGrowTime && !holded && !shrink)
        {
            float velocityMagnitude = velotown.magnitude; // Get the magnitude of the velocity
            if (velocityMagnitude > 0)
            {
                // Adjust growth rate based on current size to slow growth as size increases
                float growthAmount = velocityMagnitude * growthRate / size * Time.deltaTime;
                if (size + growthAmount > maxSize)
                {
                    size = maxSize;
                }
                else
                {
                    size += growthAmount;
                }    
                transform.localScale = new Vector3(size, size, size); // Apply new size
            }
        }
    }
    #region touchytime
    //is touching ground
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Snow" && !holded)
        {
            StartCoroutine(EndOfLife());
        }
    }
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

    private IEnumerator EndOfLife()
    {
        while (true)
        {
            // Wait until the snowball is not held to start the end-of-life timer
            while (holded)
            {
                yield return null; // Wait for the next frame
            }

            float shrinkTimer = 0f;

            while (shrinkTimer < shrinkTime)
            {
                // If the snowball is picked up, reset the timer
                if (holded)
                {
                    yield return null;
                    break;
                }

                shrinkTimer += Time.deltaTime;
                yield return null;
            }

            if (!holded && shrinkTimer >= shrinkTime)
            {
                float initialSize = size;
                float elapsedTime = 0;

                while (size > 0)
                {
                    if (holded)
                    {
                        yield return null;
                        shrink = false;
                        break;
                    }
                    shrink = true;
                    elapsedTime += Time.deltaTime;
                    float shrinkAmount = Mathf.Lerp(initialSize, 0, elapsedTime / shrinkTime);
                    size = shrinkAmount;
                    transform.localScale = new Vector3(size, size, size);
                    yield return null;
                }
            }




            if(size <= 0)
            {
                // Destroy or deactivate the snowball after shrinking
                gameObject.SetActive(false);
                handSpawnKey.amountBalls--;
                yield break;
            }
            
        }
    }

}
