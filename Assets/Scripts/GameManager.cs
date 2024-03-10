using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Text Properties")]
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI finalKills;
    [SerializeField] public TextMeshProUGUI _coinText_Shop;
    [SerializeField] public TextMeshProUGUI _coinText_HUD;

    [SerializeField] private int numOfLives = 3;
    [SerializeField] private GameObject tryAgainButton;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private int killCounter = 0;
    
    [Header("Audio Properties")]
    AudioSource audioSource1;
    AudioSource audiosource2;
    public AudioClip audio1;
    public AudioClip audio2;
  
    [Header("UI Properties")]
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private SelectionManager _selectionManager;
    [SerializeField] private UI_Inventory _uiInventory;
    [SerializeField] private GameObject _uiHUD;
    [SerializeField] private CoinCounter _coinCounter;


    [Header("READONLY - Values for debugging")]
    [SerializeReference] public float timePassed = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        audioSource1 = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        _shopManager.IsShopOpen += OnShopStatusChanged;
    }

    private void OnShopStatusChanged(bool obj)
    {
        if (obj)
        {
            //The shop is opened. 
            _selectionManager.IsBlocked = true;
            if (_uiInventory.gameObject.activeInHierarchy)
                _uiInventory.gameObject.SetActive(false);
            if (_uiHUD.activeInHierarchy)
                _uiHUD.gameObject.SetActive(false);
          
            return;
        }
        //Shop has closed.
        _selectionManager.IsBlocked = false;
        _uiHUD.SetActive(true);
        //_uiInventory.gameObject.SetActive(true);
    }

    void Start()
    {
        livesText.text = numOfLives.ToString();
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
        livesText.text = numOfLives.ToString();
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

    public void AddLives(int num = 1){
        numOfLives += num;
        livesText.text = numOfLives.ToString();
        Debug.Log("You gained a life");
    }

    public void CoinValueChange(string value)
    {
        //Update the shop UI
        _coinText_Shop.text = $"Coins: {value}";

        //Update the HUD UI
        _coinText_HUD.text = value;
    }
}
