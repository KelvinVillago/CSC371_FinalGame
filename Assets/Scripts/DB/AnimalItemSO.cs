using System;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "AnimalItemSO", menuName = "Item Database SO / New Animal Item", order = 4)]
public class AnimalItemSO : ScriptableObject
{
    [Tooltip("The ID of the item, to keep things unique")]
    [field: SerializeField] public int ID { get; private set; }

    [Tooltip("The name of the item")]
    [field: SerializeField] public string Name { get; private set; }

    [Tooltip("The description of the item. It will show in the shop")]
    [field: SerializeField] public String Description { get; private set; }

    [Tooltip("The Size of the item, default is 1x1")]
    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;

    [Tooltip("Is the item placeable, used to filter")]
    [field: SerializeField] public bool IsBreakable { get; private set; }

    [Tooltip("Current Amount in inventory")]
    [field: SerializeField] public int Durability { get; internal set; }

    [Tooltip("Shop price to purchase item")]
    [field: SerializeField] public int BuyPrice { get; internal set; }

    [Tooltip("Sell back price for item")]
    [field: SerializeField] public int SellBackPrice { get; internal set; }

    [Tooltip("A maximum amount the shop can hold of this item at a time (important for buybacks? or overbuying?)")]
    [field: SerializeField] public int ShopQuantity { get; internal set; }

    [Tooltip("Current Amount in inventory")]
    [field: SerializeField] public int InventoryQuantity { get; internal set; }

    [Tooltip("The refrence to the item icon image")]
    [field: SerializeField] public GameObject Image { get; private set; }

    [Tooltip("The refrence to the prefab of the object")]
    [field: SerializeField] public GameObject Prefab { get; private set; }
}

