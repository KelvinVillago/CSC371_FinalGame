using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    AudioSource _audioSource;
    [Tooltip("Boundary in the positive and negative x direction for the player")]
    [SerializeField] private float _boundX;
    [Tooltip("Boundary in the positive and negative z direction for the player")]
    [SerializeField] private float _boundZ;

    private Animator _anim = null;
    public Vector3 _direction;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {

        if (transform.position.z > _boundZ || transform.position.z < -_boundZ || transform.position.x > _boundX || transform.position.x < -_boundX)
        {
            if (transform.position.z > _boundZ)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, _boundZ);
            }
            if (transform.position.z < -_boundZ)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -_boundZ);
            }
            if (transform.position.x > _boundX)
            {
                transform.position = new Vector3(_boundX, transform.position.y, transform.position.z);
            }
            if (transform.position.x < -_boundX)
            {
                transform.position = new Vector3(-_boundX, transform.position.y, transform.position.z);
            }
        }
    }


}
