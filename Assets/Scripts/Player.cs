using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    AudioSource _audioSource;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > 50 || transform.position.z < -50 || transform.position.x > 50 || transform.position.x < -50)
        {
            if (transform.position.z > 50)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 50);
            }
            if (transform.position.z < -50)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -50);
            }
            if (transform.position.x > 50)
            {
                transform.position = new Vector3(50, transform.position.y, transform.position.z);
            }
            if (transform.position.x < -50)
            {
                transform.position = new Vector3(-50, transform.position.y, transform.position.z);
            }
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Enemy"))
    //     {
    //         _audioSource.PlayOneShot(_audioSource.clip);
    //         GameManager.Instance.RemoveLife();
    //         //AudioPause.audioInstance.RemoveLife();
    //         Destroy(other.gameObject);
    //     }
    // }
}
