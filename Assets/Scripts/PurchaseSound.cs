using UnityEngine;
using UnityEngine.UI;

public class PurchaseSound : MonoBehaviour
{
    public AudioClip buttonClickSound;
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to the button prefab
        audioSource = GetComponent<AudioSource>();

        // Set the AudioClip to play
        if (buttonClickSound != null)
        {
            audioSource.clip = buttonClickSound;
        }

        // Add a Button component if not already present
        if (GetComponent<Button>() == null)
        {
            gameObject.AddComponent<Button>();
        }

        // Add a listener to the Button component's onClick event
        GetComponent<Button>().onClick.AddListener(PlayButtonClickSound);
    }

    // Method to play the sound effect when the button is pressed
    void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
