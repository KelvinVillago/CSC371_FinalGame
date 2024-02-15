using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform spawn1;
    public Transform spawn2;
    public Transform spawn3;
    public Transform spawn4;
    public Transform spawn5;
    public Transform spawn6;
    public Transform spawn7;
    public Transform spawn8;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnRate;
    [SerializeField] private bool canSpawn = true;
    public float timePassed = 0f;

    // Start is called before the first frame update
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
        int choice = Random.Range(0, 8);
        int rand = Random.Range(0, enemyPrefabs.Length);
        GameObject enemySpawned = enemyPrefabs[rand];
        switch(choice)
        {
            case 0:
                gameObject = Instantiate(enemySpawned, spawn1.position, Quaternion.identity);
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;

            case 1:
                gameObject = Instantiate(enemySpawned, spawn2.position, Quaternion.identity);
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;

            case 2:
                gameObject = Instantiate(enemySpawned, spawn3.position, Quaternion.identity);
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;

            case 3:
                gameObject = Instantiate(enemySpawned, spawn4.position, Quaternion.identity);
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            
            case 4:
                gameObject = Instantiate(enemySpawned, spawn5.position, Quaternion.identity);
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            
            case 5:
                gameObject = Instantiate(enemySpawned, spawn6.position, Quaternion.identity);
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            
            case 6:
                gameObject = Instantiate(enemySpawned, spawn7.position, Quaternion.identity);
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            
            case 7:
                gameObject = Instantiate(enemySpawned, spawn8.position, Quaternion.identity);
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;

        }

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
