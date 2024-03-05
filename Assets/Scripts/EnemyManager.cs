using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnRate;
    [SerializeField] private bool canSpawn = true;
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

    void Start()
    {
        // Using Coroutines
        StartCoroutine(SpawnEnemyCoroutine());
    }
    void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= 20)
        {
            spawnRate = 1;
        }
        if(timePassed >= 40)
        {
            spawnRate = 0.5f;
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(sheepLocation.position, radius);
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
}
