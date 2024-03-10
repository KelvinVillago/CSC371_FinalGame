using UnityEngine;
using System.Collections.Generic;

public class ChickenController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private GameObject targetCoin;
    private List<GameObject> coins;
    public CoinCounter a;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coins = new List<GameObject>();
        a = GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<CoinCounter>();
    }

    void Update()
    {
        if (targetCoin != null)
        {
            Vector3 direction = targetCoin.transform.position - transform.position;
            direction.y = 0; // Ensure the chicken moves only in the horizontal plane
            direction.Normalize();
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            //Add the value of a collected coin... change this to add a value to the coin
            a.ChangeBalance(50);
            coins.Remove(other.gameObject); // Remove collected coin from the list
            Destroy(other.gameObject); // Collect the coin
            // Optionally, you can play a sound, increase score, etc.
        }
    }

    public void AddCoin(GameObject coin)
    {
        coins.Add(coin);
        if (targetCoin == null)
        {
            SetNearestCoinAsTarget();
        }
    }

    public void RemoveCoin(GameObject coin)
    {
        coins.Remove(coin);
        if (coin == targetCoin)
        {
            SetNearestCoinAsTarget();
        }
    }

    private void SetNearestCoinAsTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestCoin = null;

        foreach (GameObject coin in coins)
        {
            float distanceToCoin = Vector3.Distance(transform.position, coin.transform.position);
            if (distanceToCoin < shortestDistance)
            {
                shortestDistance = distanceToCoin;
                nearestCoin = coin;
            }
        }

        targetCoin = nearestCoin;
    }
}
