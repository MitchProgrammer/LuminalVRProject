using UnityEngine;
using UnityEngine.UI;

public class ToggleVoice : MonoBehaviour
{
    public Toggle toggle;
    public AudioSource audioSource;
    public GameObject subtitleText;

    void Start()
    {
        // Play the audio source when the scene starts
        audioSource.Play();
        subtitleText.SetActive(true);

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
        // Play or pause the audio source based on the toggle's value
        if (change.isOn)
        {
            audioSource.Play();
            subtitleText.SetActive(true);
        }
        else
        {
            audioSource.Pause();
            subtitleText.SetActive(false);
        }
    }
}
