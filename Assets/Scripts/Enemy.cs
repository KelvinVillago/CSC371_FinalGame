using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    GameObject coin;
    //AudioSource audioPlayer;
    public AudioClip coinClip;

    [SerializeField] float _speed;
    [SerializeField] float _rightBoundary;
    [SerializeField] float _leftBoundary;
    [SerializeField] float _topBoundary;
    [SerializeField] float _downBoundary;
    [SerializeField] GameObject prefab;
    public Transform target;
    public Transform spawnPoint;
    [SerializeField] private LineRenderer lineRenderer = null;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
      //audioPlayer = GetComponent<AudioSource>();
        lineRenderer.positionCount = 2;
    }


    void Update()
    {
        if(!target)
        {
            getTarget();
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
        // if(transform.position.x < _rightBoundary)
        // {
        //     Destroy(gameObject);
        // }
        // if (transform.position.x > _leftBoundary)
        // {
        //     Destroy(gameObject);
        // }
        // if (transform.position.y < _downBoundary)
        // {
        //     Destroy(gameObject);
        // }
        // if (transform.position.y > _topBoundary)
        // {
        //     Destroy(gameObject);
        // }
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            //audioPlayer.Play();
            AudioSource.PlayClipAtPoint(coinClip, transform.position);
            Destroy(this.gameObject);
            Instantiate(prefab, transform.position, Quaternion.identity);
            // GameManager.Instance.AddKill();
        }
        // if(other.CompareTag("Sheep"))
        // {
        //     Destroy(other.gameObject);
        //     GameManager.Instance.RemoveLife();
        //     print("sheep hit");
        // }
    }

    private void getTarget()
    {
        if(GameObject.FindGameObjectWithTag("Sheep"))
        {
            target = GameObject.FindGameObjectWithTag("Sheep").transform;

            Vector3 relativePos = target.position - transform.position;

            // the second argument, upwards, defaults to Vector3.up
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;
        }
    }
}
