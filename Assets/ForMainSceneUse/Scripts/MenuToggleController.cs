using UnityEngine;
using UnityEngine.UI; // Ensure you include this for UI elements

public class MenuToggleController : MonoBehaviour
{
    [System.Serializable]
    public class ToggleObjectPair
    {
        public Toggle toggle;
        public GameObject targetObject;
    }

    public ToggleObjectPair[] toggleObjectPairs;

    private void Start()
    {
        // Initialize the toggles and their corresponding game objects
        InitializeToggles();
    }

    private void InitializeToggles()
    {
        foreach (var pair in toggleObjectPairs)
        {
            if (pair.toggle != null && pair.targetObject != null)
            {
                // Set the initial state of the target object based on the toggle's value
                pair.targetObject.SetActive(pair.toggle.isOn);

                // Add a listener to the toggle to call the ToggleObject method when its value changes
                pair.toggle.onValueChanged.AddListener(isOn =>
                {
                    ToggleObject(pair.targetObject, isOn);
                });
            }
        }
    }

    private void ToggleObject(GameObject targetObject, bool isOn)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(isOn);
        }
    }
}
