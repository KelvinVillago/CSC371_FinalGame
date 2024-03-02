using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Scribtable Objects / Item / New Shop Item", order = 2)]
public class ShopItemSO2 : ItemSO
{
    [field: Header("Shop Item Properties")]

    [Tooltip("Price to purchase item")]
    [field: SerializeField] public int BuyPrice { get; private set; }

    [Tooltip("Amount given for selling item to the shop")]
    [field: SerializeField] public int SellBackPrice { get; private set; }

    [Tooltip("A maximum amount the shop can hold of this item at a time (important for buybacks? or overbuying?)")]
    [field: SerializeField] public int ShopQuantity { get; private set; }

    [field: Tooltip("Level item is placed in shop, 0 = never")]
    [field: SerializeField] public int AvailableLevel { get; private set; } = 1;
}
