using UnityEngine;
using UnityEngine.UI;

public class ToggleVoice : MonoBehaviour
{
    public Toggle toggle;
    public AudioSource voiceThing;
    public GameObject subtitleText;
    public UIManager annoyingScript;

    void Start()
    {
        // Play the audio source when the scene starts
        voiceThing.Play();
        subtitleText.SetActive(true);
        annoyingScript.isPaused = false;

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
            voiceThing.Play();
            subtitleText.SetActive(true);
            annoyingScript.isPaused = false;
        }
        else
        {
            voiceThing.Pause();
            subtitleText.SetActive(false);
            annoyingScript.isPaused = true;
        }
    }
}
