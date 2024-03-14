using System;
using UnityEngine;

/***
 *  PlayerInventory: Sets the players inventory, Handles how player interacts with inventory items.
 */
[Serializable]
public class PlayerInventory : MonoBehaviour
{
    [Tooltip("The UI_Inventory panel the player will use to display their inventory")]
    [SerializeField] private UI_Inventory _uiInventory;
    private Transform _gunSlot;
    [field:Header("READONLY: For Debugging")]
    [SerializeField] private Inventory _inventory;
    public Inventory Inventory {  get { return _inventory; } }
    private void Awake()
    {
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
        if(_gunSlot == null)
        {
            //For testing some items are not nested in the game object. 
            return;
        }

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
        if (currentGun.name == item.itemSO.Name)
        {
            //Already Equipt dont remake.
            return;
        }

        //Remove the current weapon.
        Destroy(_gunSlot.GetChild(0).gameObject);

        //Equipt the new Weapon
        newGun = Instantiate(item.itemSO.Prefab, _gunSlot);
        newGun.name = item.itemSO.Name;
        
        if(newGun.name == "Sniper Rifle")
        {
            //Turn the camera back on.
            gameObject.GetComponent<SimplePlayerController>().sniperRifle = newGun;

        }
        if(newGun.name == "Shot Gun"){
            gameObject.GetComponent<SimplePlayerController>().shotGun = newGun;
        }
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
        //Item removeOne = new Item(item.itemSO, 1);
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

    public Vector3 GetPos()
    {
        return gameObject.transform.position;
    }

    private void OnDestroy()
    {
        _inventory.EquipItemAction -= EquipWeapon;
    }
}
