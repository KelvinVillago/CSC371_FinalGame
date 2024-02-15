using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // reload gun
    InputAction _reloadAction;

    void Awake()
    {
        // input
        PlayerInput playerInput = GetComponent<PlayerInput>();
        _reloadAction = playerInput.actions["reload"];

        // make sure magazine is full
        _bulletsLeft = magazineSize;
        _readyToShoot = true;
    }

    void Update()
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
        if (allowButtonHold) _shooting = Mouse.current.leftButton.isPressed;
        else _shooting = Mouse.current.leftButton.wasPressedThisFrame;

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
    }
}
