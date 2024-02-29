using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DefenseItemSO", menuName = "Item Database SO / New Defense Item", order = 3)]
public class DefenseItemSO : ItemSO
{
    [Tooltip("The Size of the item, default is 1x1")]
    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;

    [Tooltip("Can the take damage, used to filter")]
    [field: SerializeField] public bool IsBreakable { get; private set; }

    [Tooltip("Current Amount in inventory")]
    [field: SerializeField] public int Durability { get; internal set; }

    [Tooltip("Shop price to purchase item")]
    [field: SerializeField] public int BuyPrice { get; internal set; }

    [Tooltip("Sell back price for item")]
    [field: SerializeField] public int SellBackPrice { get; internal set; }
}
