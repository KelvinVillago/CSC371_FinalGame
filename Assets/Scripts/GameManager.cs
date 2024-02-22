using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI finalKills;
    [SerializeField] private int numOfLives = 3;
    public static GameManager Instance;
    [SerializeField] private GameObject tryAgainButton;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private int killCounter = 0;
    AudioSource audioSource1;
    AudioSource audiosource2;
    public AudioClip audio1;
    public AudioClip audio2;
    public float timePassed = 0;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        audioSource1 = GetComponent<AudioSource>();
    }

    void Start()
    {
        livesText.text = "Sheep Count: " + numOfLives;
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= 2)
        {
            startScreen.SetActive(false);
        }
        killsText.text = "Time: " + Mathf.FloorToInt(Time.timeSinceLevelLoad);
    }

    public void RemoveLife(int num = 1)
    {
        numOfLives -= num;
        livesText.text = "Sheep Count: " + numOfLives;
        Debug.Log("You lost a life");
        // if(numOfLives == 1f)
        // {
        //     deathParticle.SetActive(true);
        // }
        if(numOfLives <= 0f)
        {
            Time.timeScale = 0;
            tryAgainButton.SetActive(true);
            finalKills.text = "Kill Count: " + killCounter;
            gameOver.SetActive(true);
            // audioSource1.clip = audio1;
            // audioSource1.PlayOneShot(audioSource1.clip);
            // audiosource2.clip = audio2;
            // audiosource2.Pause();
        }
    }

    // public void AddKill(int num = 1)
    // {
    //     killCounter += 1;
    //     killsText.text = "Kills: " + killCounter;  
    // }
}
