using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scene Names")]
    public string menuScene = "Menu";
    public string gameScene = "Game";

    [Header("Fade Settings")]
    public Material fadeMaterial; // Material to use for fading
    public float fadeDuration = 1.0f;

    [Header("Game Settings")]
    public float gameDuration = 5.0f; // Default game duration in minutes

    [SerializeField, Tooltip("Time remaining in seconds, visible in the Inspector.")]
    private float timeRemaining; // Now serialized so we can view it in the Inspector

    private bool isFading = false;
    private bool gameStarted = false;

    public GameObject menuCanvas; // Reference to the Menu Canvas

    [Header("Audio")]
    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip buttonClickSound; // Clip to play when buttons are clicked

    private void Awake()
    {
        // Ensure singleton instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance
        }
    }

    private void Start()
    {
        // Ensure MenuCanvas is active in the Menu scene
        HandleCanvasActivation();
    }

    private void OnEnable()
    {
        // Listen for scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from scene change event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Handle canvas activation when a new scene is loaded
        HandleCanvasActivation();
    }

    private void HandleCanvasActivation()
    {
        // Check if we are in the Menu scene, activate or deactivate the menu canvas accordingly
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            if (menuCanvas != null)
            {
                menuCanvas.SetActive(true); // Activate the MenuCanvas in the Menu scene
            }
        }
        else
        {
            if (menuCanvas != null)
            {
                menuCanvas.SetActive(false); // Deactivate the MenuCanvas in other scenes
            }
        }
    }

    private void Update()
    {
        if (gameStarted)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                StartCoroutine(FadeAndLoadScene(menuScene));
                gameStarted = false;
            }
        }
    }

    // Call this when a UI button is pressed to set the time and start the game
    public void SetGameDurationAndStart(float durationInMinutes)
    {
        gameDuration = durationInMinutes;
        timeRemaining = gameDuration * 60; // Convert minutes to seconds
        gameStarted = true; // Set game as started
        StartCoroutine(FadeAndLoadScene(gameScene));
    }

    // This coroutine fades out, loads the next scene, and fades back in
    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        yield return StartCoroutine(FadeOut()); // Fade out before loading the scene
        SceneManager.LoadScene(sceneName);      // Load the scene
        yield return StartCoroutine(FadeIn());  // Fade in after loading
    }

    // Fade out function
    private IEnumerator FadeOut()
    {
        if (fadeMaterial != null && !isFading)
        {
            isFading = true;
            float timer = 0f;
            Color color = fadeMaterial.color;
            color.a = 0f; // Ensure starting alpha is 0

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration); // Fade alpha from 0 to 1
                fadeMaterial.color = color;
                yield return null;
            }

            color.a = 1f; // Ensure alpha is fully opaque at the end
            fadeMaterial.color = color;
            isFading = false;
        }
    }

    // Fade in function
    private IEnumerator FadeIn()
    {
        if (fadeMaterial != null && !isFading)
        {
            isFading = true;
            float timer = 0f;
            Color color = fadeMaterial.color;
            color.a = 1f; // Ensure starting alpha is 1

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration); // Fade alpha from 1 to 0
                fadeMaterial.color = color;
                yield return null;
            }

            color.a = 0f; // Ensure alpha is fully transparent at the end
            fadeMaterial.color = color;
            isFading = false;
        }
    }

    // Call this function to quit the game and return to the menu
    public void EndGameAndReturnToMenu()
    {
        StartCoroutine(FadeAndLoadScene(menuScene));
    }

    // Helper function to play button click sound
    private void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    public void OnThreeMinuteButtonClick()
    {
        PlayButtonClickSound();
        GameManager.Instance.SetGameDurationAndStart(3);
    }

    public void OnFiveMinuteButtonClick()
    {
        PlayButtonClickSound();
        GameManager.Instance.SetGameDurationAndStart(5);
    }

    public void OnTenMinuteButtonClick()
    {
        PlayButtonClickSound();
        GameManager.Instance.SetGameDurationAndStart(10);
    }
}
