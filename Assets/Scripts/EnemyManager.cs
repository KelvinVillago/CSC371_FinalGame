using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnRate;
    private bool canSpawn;
    private bool waveOver;
    public float timePassed = 0f;
    [SerializeField] private float radius;
    public Transform sheepLocation;
    
    [SerializeField] private GameObject[] wave1;
    [SerializeField] private GameObject[] wave2;
    [SerializeField] private GameObject[] wave3;
    [SerializeField] private GameObject[] wave4;
    [SerializeField] private GameObject[] wave5;
    [SerializeField] private GameObject[] wave6;
    [SerializeField] private GameObject[] wave7;
    [SerializeField] private GameObject[] wave8;
    private int waveNum = 1;
    public TextMeshProUGUI waveText;
    public GameObject startWave;
    public GameObject inventoryBtn;
    public GameObject shopBtn;
    public Light dirLight;
    
    void Start()
    {
        // Using Coroutines
        //StartCoroutine(SpawnEnemyCoroutine());
        canSpawn = true;
        waveOver = false;
    }
    void Update()
    {
        GameObject[] currentWave;
        switch(waveNum)
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
        if(canSpawn)
        {
            StopCoroutine(EnemyDefeat());
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
        dirLight.intensity = 0.2f;
        startWave.SetActive(false);
        shopBtn.SetActive(false);
        inventoryBtn.SetActive(false);
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
        waveOver = false;
        startWave.SetActive(false);
        shopBtn.SetActive(false);
        inventoryBtn.SetActive(false);
        waveText.text = "Wave: " + waveNum.ToString() + "/8";
        canSpawn = false;
        for(int i = 0; i < wave.Length; i++){
            yield return new WaitForSeconds(spawnRate);
            SpawnEnemyPrefab(wave[i]);
        }
        waveNum++;
        waveOver = true;
        if(waveNum > 8){
            waveNum = 1;
        }
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(EnemyDefeat());
    }

    IEnumerator EnemyDefeat()
    {
        bool check1 = false;
        bool check2 = false;
        while(canSpawn == false)
        {
            GameObject[] enemiesRemaining;
            enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy");
            if(enemiesRemaining.Length == 0){
                check1 = true;
            }

            yield return new WaitForSeconds(2.0f);

            enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy");
            if(enemiesRemaining.Length == 0){
                check2 = true;
            }

            if(check1 == true && check2 == true && waveOver == true){
                yield return new WaitForSeconds(1.0f);
                startWave.SetActive(true);
                shopBtn.SetActive(true);
                inventoryBtn.SetActive(true);
                dirLight.intensity = 1;
            }

        }
    }

    
}
