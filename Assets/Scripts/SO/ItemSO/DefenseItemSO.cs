using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewDefenseItemSO", menuName = "Scribtable Objects / Item / New Defense Item", order = 5)]
public class DefenseItemSO : ShopItemSO2
{
    [field: Header("Defense Item Properties")]
    [field: Tooltip("The Size of the item, default is 1x1")]
    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;

    [Tooltip("Amount of damage before breaking")]
    [field: SerializeField] public int Durability { get; internal set; }
}
