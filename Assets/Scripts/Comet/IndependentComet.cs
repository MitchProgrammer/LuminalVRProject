using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndependentComet : MonoBehaviour
{
    public GameObject endPlace;
    public float moveSpeed;
    private ParticleSystem cometParticles;

    private void Start()
    {
        cometParticles = GetComponent<ParticleSystem>();

        MoveToLocation(); //test
    }

    public void MoveToLocation()
    {
        StartCoroutine(MoveTowardsTarget());
    }

    private IEnumerator MoveTowardsTarget()
    {
        while (Vector3.Distance(transform.position, endPlace.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPlace.transform.position, moveSpeed * Time.deltaTime);

            yield return null;
        }

        if (cometParticles != null && cometParticles.isPlaying)
        {
            cometParticles.Stop();
        }
    }

}
