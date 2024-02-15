using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 2f;
    [SerializeField] ParticleSystem hitPrefab;

    float _bulletLifetimeInSeconds = 4f;
    Rigidbody _rgbd;

    // Start is called before the first frame update
    void Awake()
    {
        _rgbd = GetComponent<Rigidbody>();
    }

    public void Setup(Vector3 shootDir)
    {
        _rgbd.velocity = shootDir * bulletSpeed;
        Destroy(gameObject, _bulletLifetimeInSeconds);
    }

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(hitPrefab, collision.GetContact(0).point, transform.rotation);
        Destroy(this.gameObject);
    }
}
