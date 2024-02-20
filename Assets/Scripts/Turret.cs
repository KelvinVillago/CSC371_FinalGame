using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    public float range = 15f;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] private LineRenderer lineRenderer = null;
    
    string enemyTag = "Enemy";
    public Transform partToRotate;
    public Transform spawnPoint;

    //Shooting
    public float fireRate = 1f;
    private float delay = 0f;
    public GameObject bullet;
    public GameObject muzzleFlash;
    public float shootForce, upwardForce;


    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float minDist = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < minDist)
            {
                minDist = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }

        if(nearestEnemy != null && minDist <= range)
        {   
            target = nearestEnemy.transform;
            lineRenderer.positionCount = 2;
        }
        else
        {
            target = null;
            lineRenderer.positionCount = 0;
        }
    }

    void Update()
    {
        if(target == null)
        {
            return;
        }

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime*rotateSpeed).eulerAngles;

        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if(delay <= 0f)
        {
            Shoot();
            delay = 1f / fireRate;
        }

        delay -= Time.deltaTime;

    }

    void LateUpdate()
    {
        if(target == null)
        {
            return;
        }

        lineRenderer.SetPosition(0, spawnPoint.position);
        lineRenderer.SetPosition(1, target.position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void Shoot()
    {
        // calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = spawnPoint.forward;

        // instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
        currentBullet.transform.forward = directionWithoutSpread.normalized;

        // add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.up * upwardForce, ForceMode.Impulse);

        // add muzzle flash
        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, spawnPoint.position, Quaternion.identity);
        }
    }
}
