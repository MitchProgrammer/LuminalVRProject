using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class ToggleSnow : MonoBehaviour
{
    public Toggle toggle;
    public ParticleSystem particle;
    
    void Start()
    {
        var emission = particle.emission;
        var main = particle.main;
        // Play the audio source when the scene starts
        emission.enabled = true;
        main.startLifetimeMultiplier = 5f;

        // Set the toggle to be on by default
        toggle.isOn = true;

        // Add a listener to call the ToggleValueChanged method when the toggle's value changes
        toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(toggle);
        });
    }

    // This method is called when the toggle's value changes
    void ToggleValueChanged(Toggle change)
    {
        var emissions = particle.emission;
        var main = particle.main;
        // Play or pause the audio source based on the toggle's value
        if (change.isOn)
        {
            emissions.enabled = true;
            main.startLifetimeMultiplier = 5f;
        }
        else
        {
            emissions.enabled = false;
            main.startLifetimeMultiplier = 0.1f;
        }
    }
}
