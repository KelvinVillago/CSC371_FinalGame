using System;
using UnityEngine;
//Changing properties of SO's does not keep changes after building.
//Here we are saving properties of items that can change

[Serializable]
public class Item 
{
    public ItemSO itemSO;
    public Type type;
    public int amount;

    public Item(ItemSO item)
    {
        type = item.GetType();
        itemSO = item;
        amount = 1;
    }

    public Item(ItemSO item, int amount)
    { 
        type = item.GetType();
        itemSO = item;
        this.amount = amount;
    }

    public bool IsSameOrSubclass(Type potentialBase)
    {
        Type potentialDescendant = this.type;
        return potentialDescendant.IsSubclassOf(potentialBase)
               || potentialDescendant == potentialBase;
    }

    public T GetItemSO<T>() where T : ItemSO
    {
        //if(type == typeof(T))
        if(IsSameOrSubclass(typeof(T)))
        {
            return (T)itemSO;
        }
        Debug.LogError("GetItemSO: Cannot Get SO, not of correct type");
        return null;
    }

    public string AmountSting()
    {
        return amount.ToString();
    }
}
