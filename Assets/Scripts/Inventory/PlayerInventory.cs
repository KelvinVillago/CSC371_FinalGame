using System;
using UnityEngine;


public class PlayerInventory : MonoBehaviour
{
    //[SerializeField] private GameObject _inventoryUIPanel;
    [Tooltip("The UI_Inventory panel the player will use to display their inventory")]
    [SerializeField] private GameObject _invPanel;
    private UI_Inventory _uiInventory;
    private Inventory _inventory;
    public Inventory Inventory {  get { return _inventory; } }

    private void Awake()
    {
        _uiInventory = _invPanel.GetComponent<UI_Inventory>();
    }
    private void Start()
    {
        _inventory = new Inventory();
        _uiInventory.SetInventory(_inventory);
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the player is touching something on the world it can collect
        ItemWorld itemWorld = other.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            //Add it to inventory
            _inventory.AddItem(itemWorld.Item);
            //Distroy it from the world
            itemWorld.DestroySelf();
        }
    }
    public GameObject GetInventoryPanel()
    {
        return _invPanel;
    }
}
