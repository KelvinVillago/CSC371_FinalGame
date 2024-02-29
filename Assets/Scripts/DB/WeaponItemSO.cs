using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "WeaponItemSO", menuName = "Item Database SO / New Weapon Item", order = 2)]
public class WeaponItemSO : ItemSO
{
    [field: Tooltip("Can the item break")]
    [field: SerializeField] public bool IsBreakable { get; private set; }

    [field: Tooltip("Damage until it breaks")]
    [field: SerializeField] public int Durability { get; private set; }

    [field: Tooltip("Shop price to purchase item")]
    [field: SerializeField] public int BuyPrice { get; private set; }

    [field: Tooltip("Sell back price for item")]
    [field: SerializeField] public int SellBackPrice { get; private set; }

    [field: Tooltip("The maximum amout of ammo the mag can hold")]
    [field: SerializeField] public int MagSize { get; private set; }
}

