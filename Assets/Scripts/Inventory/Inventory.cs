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
            if (removedItem.itemSO == inventoryItem.itemSO)
            {
                Debug.Log("RemoveItem: RemovedItem.Amount = " + removedItem.amount);
                //Already exists.
                if (inventoryItem.amount < removedItem.amount)
                {
                    removedItem.amount = -inventoryItem.amount;
                    _items.Remove(inventoryItem);
                    //Keep looking for another stack to remove from.
                }
                else if (inventoryItem.amount == removedItem.amount)
                {
                    _items.Remove(inventoryItem);
                    removed = true;
                    //no more to remove leave the loop;
                    break;
                }
                else
                {
                    inventoryItem.amount -= removedItem.amount;
                    removed = true;
                }
            }
        }
        if(removed == true)
        {
            OnInventoryChanged?.Invoke();
            //Debug.Log(_items.Count);
        }
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
        Debug.Log("Inventory Equipt");
        EquipItemAction?.Invoke(slot);
    }
}
