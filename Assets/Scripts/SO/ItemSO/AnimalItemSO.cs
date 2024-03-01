using System;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "NewAnimalItemSO", menuName = "Scribtable Objects / Item / New Animal Item", order = 6)]
public class AnimalItemSO : ShopItemSO2
{
    [field: Header("Animal Item Properties")]
    [field: Tooltip("The max life of the animal")]
    [field: SerializeField] public float Maxlife { get; private set; } = 100f;
}

