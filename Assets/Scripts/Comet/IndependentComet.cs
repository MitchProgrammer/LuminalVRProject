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
    public int emissionIntensity = 3;
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

        Color finalEmissionColor = colours[randomIndex] * Mathf.LinearToGammaSpace(emissionIntensity);

        Material particleMat = cometParticles.GetComponent<Renderer>().material;
        particleMat.SetColor("_EmissionColor", finalEmissionColor);

        Material babyParticleMat = babyParticles.GetComponent<Renderer>().material;
        babyParticleMat.SetColor("_EmissionColor", finalEmissionColor);

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
