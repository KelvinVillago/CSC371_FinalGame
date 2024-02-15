using UnityEngine;

public class SheepController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameManager.Instance.RemoveLife();
            Destroy(other.gameObject);
        }
    }
}
