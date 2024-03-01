using UnityEngine;


public class PlayerInventory : MonoBehaviour
{
    //[SerializeField] private GameObject _inventoryUIPanel;
    [SerializeField] private UI_Inventory _uiInventory;
    private Inventory _inventory;

    private void Awake()
    {
        //_inventory = new Inventory();
    }
    private void Start()
    {
        _inventory = new Inventory();
        _uiInventory.SetInventory(_inventory);
    }

}
