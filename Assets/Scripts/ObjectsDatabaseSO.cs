using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectsData;
}

[Serializable]
public class ObjectData
{
    //public int MyProperty { get; set; }
    //public getter = other scripts can get
    //private setter = It can only be set in the serializedField (in the inspector).


    //Update in inspector only
    [Tooltip("The ID of the item, to keep things unique")]
    [field: SerializeField] public int ID { get; private set; }

    [Tooltip("The name of the item")]
    [field: SerializeField] public string Name { get; private set; }

    [Tooltip("The description of the item. It will show in the shop")]
    [field: SerializeField] public String Description { get; private set; }

    [Tooltip("The Size of the item, default is 1x1")]
    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;
    
    [Tooltip("The refrence to the prefab of the object")]
    [field: SerializeField] public GameObject Prefab { get; private set; }

    //Shop information - update anywhere
    [Tooltip("Unlocked and avalible for purchase in shop")]
    [field: SerializeField] public bool IsAvalibleInShop { get; internal set; }

    [Tooltip("Shop price to purchase item")]
    [field: SerializeField] public int BuyPrice { get; internal set; }
    
    [Tooltip("Sell back price for item")]
    [field: SerializeField] public int SellBackPrice { get; internal set; }
    
    [Tooltip("A maximum amount the shop can hold of this item at a time (important for buybacks? or overbuying?)")]
    [field: SerializeField] public int ShopQuantity { get; internal set; }

    [Tooltip("Current Amount in inventory")]
    [field: SerializeField] public int InventoryQuantity { get; internal set; }
}