using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    public float bulletLifetime = 4f;
    public ParticleSystem hitExplosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(hitExplosionPrefab, collision.GetContact(0).point, transform.rotation);
        Destroy(this.gameObject);
    }
}