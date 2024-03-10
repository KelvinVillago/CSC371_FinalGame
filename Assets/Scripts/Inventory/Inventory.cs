using System;
using System.Collections.Generic;
using UnityEngine;

//This will not be a monobehavior it is a simple class
public class Inventory 
{
    private List<Item> _items;
    public event Action OnInventoryChanged;
    public event Action<Item> UseItemAction;
    public event Action<Transform> EquipItemAction;
    public Inventory(Action<Item> UseItemAction) 
    {
        //Passing the function so the inventory does need to be connected to the player
        this.UseItemAction = UseItemAction;
        //Initalize the players lists
        _items = new List<Item>();
    }

    public void AddItem(Item newItem)
    {
        //If the item is stackable....
        if(newItem.itemSO.StackAmount > 1)
        {
            bool isAlreadyInInventory = false;
            // Check if its already in the inventory
            foreach(Item inventoryItem in _items)
            {
                if(newItem.itemSO == inventoryItem.itemSO 
                    && inventoryItem.amount + newItem.amount <= inventoryItem.itemSO.StackAmount)
                {
                    //Debug.Log("AddItem: NewItem.Amount = " + newItem.amount);
                    //Already exists.
                    inventoryItem.amount += newItem.amount;
                    isAlreadyInInventory = true;
                }
            }
            if (!isAlreadyInInventory)
            {
                _items.Add(newItem);
            }
        }
        else
        {
            _items.Add(newItem);
        }
        OnInventoryChanged?.Invoke();
        //Debug.Log(_items.Count);
    }
    public void RemoveItem(Item removedItem)
    {
        bool removed = false;
        // Check if its already in the inventory
        foreach (Item inventoryItem in _items)
        {
            if(removedItem.SlotID == inventoryItem.SlotID)
            {
                //Debug.Log("RemoveItem: RemovedItem.Amount = " + removedItem.amount);
                //Already exists.
                if(inventoryItem.amount > 0)
                {
                    inventoryItem.amount -= 1;
                    removed = true;

                    if (inventoryItem.amount == 0)
                    {
                        //Item removeOne = new Item(removedItem.itemSO, 1);
                        _items.Remove(inventoryItem);
                    }
                    break;
                }
            }
        }
        if(removed == true)
        {
            OnInventoryChanged?.Invoke();
        }
    }

    public int getIndexByMatchItemSO(Item item)
    {
        int index = -1;
        //Look for the item with matching itemSO
        index = _items.FindIndex(data => data.itemSO == item.itemSO);
        return index;
    }

    public Item getItemByMatchItemSO(Item item)
    {
        int index = getIndexByMatchItemSO(item);
        if (index < 0)
        {
            Debug.Log($"No matching SO found: {item.itemSO}");
            return null;
        }
        return _items[index];
    }

    public void UseItem(Item item)
    {
        UseItemAction(item);
    }

    public List<Item> GetItemsList() 
    {
        return _items;
    }

    internal void EquipItem(Transform slot)
    {
        EquipItemAction?.Invoke(slot);
    }
}
