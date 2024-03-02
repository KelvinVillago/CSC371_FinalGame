using System;
using System.Collections.Generic;
using UnityEngine;

//This will not be a monobehavior it is a simple class
public class Inventory 
{
    private List<Item> _items;
    public event Action<Item, bool> OnInventoryChanged;
        
    public Inventory() 
    {
        //Initalize the players lists
        _items = new List<Item>();
    }

    public void AddItem(Item newItem)
    {
        if(newItem.itemSO.StackAmount > 1)
        {
            bool isAlreadyInInventory = false;
            //If it is stackable, check if its already in the inventory
            foreach(Item inventoryItem in _items)
            {
                if(newItem.itemSO == inventoryItem.itemSO)
                {
                    Debug.Log("AddItem: NewItem.Amount = " + newItem.amount);
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
        OnInventoryChanged?.Invoke(newItem, false);
        Debug.Log(_items.Count);
    }
    public void RemoveItem(Item removedItem)
    {
        OnInventoryChanged?.Invoke(removedItem, true);
    }
    public List<Item> GetItemsList() 
    {
        return _items;
    }
}
