using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;

public class VRMenuController : MonoBehaviour
{
    public Canvas menuCanvas;  // Reference to the Canvas

    void Start()
    {
        // Ensure the canvas is inactive initially
        menuCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        var secondaryInput = VRDevice.Device.SecondaryInputDevice;

        // Activate the menu on input from either the primary or secondary trigger
        if (secondaryInput.GetButtonDown(VRButton.One))
        {
            ActivateMenu();
        }

        if (Input.GetKeyDown(KeyCode.M))  // M key on the keyboard
        {
            ActivateMenu();
        }
    }

    void ActivateMenu()
    {
        // Always set the canvas as active
        menuCanvas.gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        // Method to close the menu from within the menu
        menuCanvas.gameObject.SetActive(false);
    }
}
