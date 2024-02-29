using System;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "AnimalItemSO", menuName = "Item Database SO / New Animal Item", order = 4)]
public class AnimalItemSO : ItemSO
{
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
}

