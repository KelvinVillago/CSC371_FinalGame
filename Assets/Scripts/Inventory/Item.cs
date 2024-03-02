using System;
using UnityEngine;
//Changing properties of SO's does not keep changes after building.
//Here we are saving properties of items that can change

public class Item 
{
    public ItemSO itemSO;
    public int amount;
    public int currentShopAmt;
    public Type type;

    public Item(ItemSO item)
    {
        Debug.Log("Item: ItemType:" + item.GetType());
        type = item.GetType();
        itemSO = item;
        amount = 1;
        currentShopAmt = 0;
    }
    public bool IsSameOrSubclass(Type potentialBase)
    {
        Type potentialDescendant = this.type;
        return potentialDescendant.IsSubclassOf(potentialBase)
               || potentialDescendant == potentialBase;
    }
}
