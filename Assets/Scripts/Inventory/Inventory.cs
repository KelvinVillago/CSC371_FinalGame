using System;
using System.Collections.Generic;
using UnityEngine;

//This will not be a monobehavior it is a simple class
public class Inventory 
{
    private List<ItemSO> _items;

    public Inventory() 
    {
        //Initalize the players lists
        _items = new List<ItemSO>();
    }

    public void AddItem(ItemSO itemSO)
    {
        _items.Add(itemSO);
        Debug.Log(_items.Count);
    }
    public List<ItemSO> GetItemsList() 
    {
        return _items;
    }
}
