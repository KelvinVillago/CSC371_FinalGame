using System;
using UnityEngine;


public class PlayerInventory : MonoBehaviour
{
    //[SerializeField] private GameObject _inventoryUIPanel;
    [Tooltip("The UI_Inventory panel the player will use to display their inventory")]
    [SerializeField] private GameObject _invPanel;
    private UI_Inventory _uiInventory;
    private Inventory _inventory;
    [SerializeField] public Transform _gunSlot;
    public Inventory Inventory {  get { return _inventory; } }


    private void Awake()
    {
        _uiInventory = _invPanel.GetComponent<UI_Inventory>();
       
    }
    private void Start()
    {
        _inventory = new Inventory(UseItem);
        _uiInventory.SetInventory(_inventory);
        _uiInventory.SetPlayer(this);
        _inventory.EquipItemAction += EquipWeapon;
    }

    private void EquipWeapon(Transform slot)
    {
        print("Equipting item");
        //Remove the current weapon.
        //Destroy(_gunSlot.GetChild(0).gameObject, 1);
        //Equipt the Weapon.
        Item item = slot.GetComponent<EquiptSlot>().Item;
        //Check for current upgrades?
        print (Instantiate(item.itemSO, _gunSlot));
        
    }

    private void UseItem(Item item)
    {
        //How Usable items effect the player..

        switch (item.itemSO.Name)
        {
            case "Energy Drink":
                print("Player drank energy drink. Do something...");
                break;
        }
       
        //Remove it 
        Item removeOne = new Item(item.itemSO, 1);
        Inventory.RemoveItem(item);
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
    public Vector3 GetPos()
    {
        return gameObject.transform.position;
    }

    private void OnDestroy()
    {
        _inventory.EquipItemAction -= EquipWeapon;
    }
}
