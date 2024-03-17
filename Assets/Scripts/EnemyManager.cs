using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnRate;
    private bool canSpawn;
    private bool waveOver;
    private bool isDay;
    public float timePassed = 0f;
    [SerializeField] private float radius;
    [SerializeField] private float timeToChange;
    public Transform sheepLocation;

    [Header("Wave Information")]
    [SerializeField] private GameObject[] wave1;
    [SerializeField] private GameObject[] wave2;
    [SerializeField] private GameObject[] wave3;
    [SerializeField] private GameObject[] wave4;
    [SerializeField] private GameObject[] wave5;
    [SerializeField] private GameObject[] wave6;
    [SerializeField] private GameObject[] wave7;
    [SerializeField] private GameObject[] wave8;
    private int waveNum = 1;
    
    [Header("UI Interactions + Audio")]
    public TextMeshProUGUI waveText;
    public GameObject startWave;
    public GameObject inventoryBtn;
    public GameObject shopBtn;
    public Light dirLight;
    public AudioClip waveStart;
    public AudioClip waveEnd;
    private AudioSource _audioSource;
    
    void Start()
    {
        // Using Coroutines
        //StartCoroutine(SpawnEnemyCoroutine());
        canSpawn = true;
        isDay = false;
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GameObject[] currentWave;
        switch(waveNum) //Uses wave number integer to determine which wave is being spawned in
        {
            case 1: 
                currentWave = wave1;
                break;
            case 2: 
                currentWave = wave2;
                break;
            case 3: 
                currentWave = wave3;
                break;
            case 4: 
                currentWave = wave4;
                break;
            case 5: 
                currentWave = wave5;
                break;
            case 6: 
                currentWave = wave6;
                break;
            case 7: 
                currentWave = wave7;
                break;
            case 8: 
                currentWave = wave8;
                break;
            default: 
                currentWave = wave1;
                break;

        }

        if(isDay)
        {
            startWave.SetActive(true);
            shopBtn.SetActive(true);
            inventoryBtn.SetActive(true);
            StartCoroutine(ChangeTime(0.2f, 1.0f));
        }
        else
        {
            startWave.SetActive(false);
            shopBtn.SetActive(false);
            inventoryBtn.SetActive(false);
            StartCoroutine(ChangeTime(1.0f, 0.2f));
        }

        if(canSpawn)
        {
            canSpawn = false;
            StartCoroutine(SpawnEnemyWave(currentWave));
        }
    }

    // Using InvokeRepeating
    void SpawnEnemy()
    {
        GameObject gameObject;
        int rand = Random.Range(0, enemyPrefabs.Length);
        GameObject enemySpawned = enemyPrefabs[rand];

        //Spawning in radius around sheep
        Vector3 spawnDir = Random.onUnitSphere;
        spawnDir.y = 0;
        spawnDir.Normalize();
        Vector3 point = sheepLocation.position;
        var spawnPosition = point + spawnDir * radius;
        gameObject = Instantiate(enemySpawned, spawnPosition, Quaternion.identity);

    }

    void SpawnEnemyPrefab(GameObject enemyPrefab)
    {
        GameObject gameObject;
        int rand = Random.Range(0, enemyPrefabs.Length);

        //Spawning in radius around sheep
        Vector3 spawnDir = Random.onUnitSphere;
        spawnDir.y = 0;
        spawnDir.Normalize();
        Vector3 point = sheepLocation.position;
        var spawnPosition = point + spawnDir * radius;
        gameObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(sheepLocation.position, radius);
    }

    public void StartWave()
    {
        StopCoroutine(EnemyDefeat());
        isDay = false;
        canSpawn = true;
    }

    // Using Coroutines
    IEnumerator SpawnEnemyCoroutine()
    {
        while (canSpawn)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnEnemy();
        }
    }

    IEnumerator SpawnEnemyWave(GameObject[] wave)
    {
        canSpawn = false;
        isDay = false;
        waveOver = false;
        waveText.text = "Wave: " + waveNum.ToString() + "/8";
        _audioSource.PlayOneShot(waveStart);

        for(int i = 0; i < wave.Length; i++){ //Spawn all prefabs in wave array
            yield return new WaitForSeconds(spawnRate);
            SpawnEnemyPrefab(wave[i]);
        }

        waveNum++;
        waveOver = true;

        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(EnemyDefeat());
    }

    IEnumerator EnemyDefeat()
    {
        while(canSpawn == false)
        {
            bool check1 = false;
            bool check2 = false;
            GameObject[] enemiesRemaining;
            enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy");
            if(enemiesRemaining.Length == 0){
                check1 = true;
            }

            yield return new WaitForSeconds(1.0f); //Delay for second check for safety

            enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy");
            if(enemiesRemaining.Length == 0){
                check2 = true;
            }

            if(check1 == true && check2 == true && waveOver == true){
                yield return new WaitForSeconds(1.0f);
                isDay = true;
                _audioSource.PlayOneShot(waveEnd);
                if(waveNum >= 8) // Set check to last wave number
                {
                    SceneManager.LoadScene("WinScreen");
                }
                break;
            }
        }
    }

    IEnumerator ChangeTime(float start, float end)
    {
        float timeElapsed = 0.0f;

        while(timeElapsed < timeToChange)
        {
            dirLight.intensity = Mathf.Lerp(start, end, timeElapsed / timeToChange);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        dirLight.intensity = end;
    }

    
}
