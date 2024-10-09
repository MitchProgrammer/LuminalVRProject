using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndependentComet : MonoBehaviour
{
    public GameObject endPlace;
    public GameObject startPlace;
    public float moveSpeed;
    public ParticleSystem cometParticles;
    public ParticleSystem babyParticles;

    public Color[] colours;
    private int lastColour;

    private void Start()
    {
        gameObject.SetActive(false);

        CometSpawn(); //test
    }

    public void CometSpawn()
    {
        int randomIndex; // random output 

        do // chooses random number, not allowed to be the last number it picked
        {
            randomIndex = Random.Range(0, colours.Length);
        } while (randomIndex == lastColour);

        cometParticles.startColor = colours[randomIndex];
        babyParticles.startColor = colours[randomIndex];

        lastColour = randomIndex;
        gameObject.SetActive(true);
        cometParticles.Play();
        babyParticles.Play();
        StartCoroutine(MoveTowardsTarget());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StopAllCoroutines();
            ResetComet();
        }
    }

    private void ResetComet()
    {
        gameObject.SetActive(false);
        transform.position = startPlace.transform.position;
        transform.rotation = startPlace.transform.rotation;
        CometSpawn();
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
            babyParticles.Stop();
            ResetComet();
        }
    }

}
