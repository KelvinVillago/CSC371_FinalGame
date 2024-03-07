using System;
using UnityEngine;

/***
 *  PlayerInventory: Sets the players inventory, Handles how player interacts with inventory items.
 */
public class PlayerInventory : MonoBehaviour
{
    [Tooltip("The UI_Inventory panel the player will use to display their inventory")]
    [SerializeField] private GameObject _playerUI;
    private UI_Inventory _uiInventory;
    private Inventory _inventory;
    private Transform _gunSlot;
    public Inventory Inventory {  get { return _inventory; } }
    private void Awake()
    {
        _uiInventory = _playerUI.GetComponentInChildren<UI_Inventory>();
        _gunSlot = transform.Find("GunSlot");
        _uiInventory.SetPlayer(this);
    }
    private void Start()
    {
        _inventory = new Inventory(UseItem);
        _inventory.EquipItemAction += EquipWeapon;
        _uiInventory.SetInventory(_inventory);
    }
    
    private void EquipWeapon(Transform slot)
    {
        Item item = slot.GetComponent<EquiptSlot>().Item;
        GameObject newGun = null;
        
        //Currently no weapons equipt
        if (_gunSlot.childCount < 1)
        {
             newGun = Instantiate(item.itemSO.Prefab, _gunSlot);
            newGun.name = item.itemSO.Name;
            return;
        }

        //Get the equiped item
        Transform currentGun = _gunSlot.GetChild(0);
        
        //Check if trying to equipt the same item
        print($"Current child: {currentGun.name}");
        if (currentGun.name == item.itemSO.Name)
        {
            print($"{currentGun.name} Already created");
            //Already Equipt dont remake.
            return;
        }

        //Remove the current weapon.
        Destroy(_gunSlot.GetChild(0).gameObject);
        //Equipt the new Weapon
        newGun = Instantiate(item.itemSO.Prefab, _gunSlot);
        newGun.name = item.itemSO.Name;
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
        return _uiInventory.gameObject;
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
