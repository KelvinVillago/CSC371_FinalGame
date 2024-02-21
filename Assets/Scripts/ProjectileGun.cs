using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Shooting with Bullets + Custom projectiles, Dave/GameDevelopment
// https://youtu.be/wZ2UUOC17AY

public class ProjectileGun : MonoBehaviour
{
    // bullet
    public GameObject bullet;

    // bullet force
    public float shootForce, upwardForce;

    // gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public float recoil = 50000000f;

    int _bulletsLeft, _bulletsShot;

    // reference
    public Transform attackPoint;

    // graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    // bug fixing
    public bool allowInvoke = true;

    // bools
    bool _shooting, _readyToShoot, _reloading;

    //parent GameObject and rigidbody
    GameObject _player;
    Rigidbody _playerRB;

    //Ammo Bar
    public Slider ammoSlider;

    //Inputs 
    private InputAction _reloadAction;
    private InputAction _shootingAction;


    void Awake()
    {
        //get parent
        _player = transform.parent.gameObject;
        _playerRB = _player.GetComponent<Rigidbody>();

        // input
        PlayerInput playerInput = _player.GetComponent<PlayerInput>();
        _reloadAction = playerInput.actions["reload"];
        _shootingAction = playerInput.actions["Shoot"];
    
        // make sure magazine is full
        _bulletsLeft = magazineSize;
        _readyToShoot = true;

        //ammo
        ammoSlider.maxValue = magazineSize;
    }

    void FixedUpdate()
    {
        MyInput();

        // Set ammo display, if it exists 
        if (ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(_bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }
    }

    void MyInput()
    {
        if (allowButtonHold)
        {
            //_shooting = Mouse.current.leftButton.isPressed;
            _shooting = _shootingAction.IsPressed();
        }
        else
        {
            //_shooting = Mouse.current.leftButton.wasPressedThisFrame;
            _shooting = _shootingAction.WasPressedThisFrame();
        }

        // reloading
        if (_reloadAction.triggered && _bulletsLeft < magazineSize && !_reloading)
        {
            Reload();
        }

        // autoreload if out of bullets
        if (_readyToShoot && _shooting && !_reloading && _bulletsLeft <= 0)
        {
            Reload();
        }

        // Shooting
        if (_readyToShoot && _shooting && !_reloading && _bulletsLeft > 0)
        {
            // set bullets shot to 0
            _bulletsShot = 0;

            Shoot();
        }
    }

    void Shoot()
    {
        _readyToShoot = false;

        // calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = attackPoint.forward;

        // calculate new direction with spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, attackPoint.rotation);
        currentBullet.transform.forward = directionWithSpread.normalized;

        // add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(attackPoint.up * upwardForce, ForceMode.Impulse);

        // add muzzle flash
        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

        _bulletsLeft--;
        _bulletsShot++;
        ammoSlider.value = _bulletsLeft;

        // invoke resetShot function (if not already invoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        // if more than one bulletsPerTap make sure to repeat shoot function
        if (_bulletsShot < bulletsPerTap && _bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }

        _playerRB.AddForce(-transform.forward * recoil * Time.deltaTime);
    }

    void ResetShot()
    {
        // Allow shooting and invoking again
        _readyToShoot = true;
        allowInvoke = true;
    }

    void Reload()
    {
        _reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    void ReloadFinished()
    {
        _reloading = false;
        _bulletsLeft = magazineSize;
        ammoSlider.value = _bulletsLeft;
    }
}
