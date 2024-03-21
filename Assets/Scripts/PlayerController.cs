using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float shootingDelay = 0.2f;
    [SerializeField] Transform bulletPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] bool useRaycast;
    [SerializeField] ParticleSystem muzzleFlashPrefab;
    [SerializeField] ParticleSystem hitPrefab;

    Camera _mainCamera;
    Animator _animator;
    AudioSource _audioSource;
    bool _pullingTrigger;
    float _nextShootTime;

    /*
     * Inspired by Code Monkey (https://youtu.be/Nke5JKPiQTw) and Jason Weimann (https://youtu.be/RCUC5-lbb2g)
     */

    void Awake()
    {
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GunControls();
    }

    void FixedUpdate()
    {
        Vector3 direction = RotateTowardsMouse();
        GunFiring(direction);
    }

    private void GunControls()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //_animator.SetBool("Shooting", true);
            _pullingTrigger = true;
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            //_animator.SetBool("Shooting", false);
            _pullingTrigger = false;
        }

        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            useRaycast = !useRaycast;
        }
    }

    private Vector3 RotateTowardsMouse()
    {
        Vector3 direction;

        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue() /*Input.mousePosition*/);

        if (Physics.Raycast(ray, out var raycastHit, Mathf.Infinity))
        {
            direction = raycastHit.point - transform.position;
        }
        else
        {
            direction = ray.GetPoint(50) - transform.position;
        }

        direction = direction.normalized;
        direction.y = 0;
        transform.forward = direction;

        return direction;
    }

    private void GunFiring(Vector3 direction)
    {
        if (!direction.Equals(Vector3.zero) && CanShoot())
        {
            if (_pullingTrigger)
            {
                //_animator.SetBool("Shooting", true);
                if (useRaycast)
                {
                    ShootRaycast(direction); // Should put all physics operations in FixedUpdate()
                }
                else
                {
                    Shoot(direction);
                }
            }
        }
    }

    bool CanShoot()
    {
        return Time.time > _nextShootTime;
    }

    void Shoot(Vector3 direction)
    {
        Debug.Log("Shooting");
        Transform bulletTransform = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        ParticleSystem muzzleFlash = Instantiate(muzzleFlashPrefab, shootPoint.position, shootPoint.rotation);
        muzzleFlash.Play();
        muzzleFlash.transform.parent = shootPoint; // so muzzle flash follows when moving
        _audioSource.Play();
        bulletTransform.GetComponent<Bullet>().Setup(direction);
        _nextShootTime = Time.time + shootingDelay;
    }

    void ShootRaycast(Vector3 direction)
    {
        ParticleSystem muzzleFlash = Instantiate(muzzleFlashPrefab, shootPoint.position, shootPoint.rotation);
        muzzleFlash.transform.parent = shootPoint; // so muzzle flash follows when moving
        _audioSource.Play();
        _nextShootTime = Time.time + shootingDelay;

        if (Physics.Raycast(shootPoint.position, direction, out var raycastHit, 30f))
        {
            Debug.Log("Hit Something");
            Instantiate(hitPrefab, raycastHit.point, transform.rotation);
        }
    }
}