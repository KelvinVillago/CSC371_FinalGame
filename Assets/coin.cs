using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private ChickenController chickenController; // Reference to the ChickenController script
    [SerializeField] private float despawnTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, despawnTime);

        // Find the ChickenController script in the scene
        chickenController = FindObjectOfType<ChickenController>();

        // Ensure the ChickenController script is found in the scene
        if (chickenController == null)
        {
            Debug.LogError("Coin script couldn't find ChickenController script in the scene.");
        }
        else
        {
            // Add this coin to the ChickenController's list of coins
            chickenController.AddCoin(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 180 * Time.deltaTime, 0);
    }

    // OnDestroy is called when the GameObject is destroyed
    void OnDestroy()
    {
        // Ensure the ChickenController script is found in the scene
        if (chickenController != null)
        {
            // Remove this coin from the ChickenController's list of coins
            chickenController.RemoveCoin(this.gameObject);
        }
    }
}
