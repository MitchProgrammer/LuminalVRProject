using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometFly : MonoBehaviour
{
    public bool goFlyComet;
    public Transform endLocation;
    public float speed = 5f;

    private void Update()
    {
        if(Input.GetKeyDown("c"))
        {
            goFlyComet = true;
        }
        if (goFlyComet)
        {
            StartCoroutine(cometFlytime());
        }
    }

    IEnumerator cometFlytime()
    {
        float finalx = endLocation.position.x;
        float finalz = endLocation.position.z;
        while(goFlyComet)
        {
            float newX = Mathf.MoveTowards(transform.position.x, finalx, speed * Time.deltaTime);
            float newZ = Mathf.MoveTowards(transform.position.z, finalz, speed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, newZ);
        }
        yield return null;
    }
}
