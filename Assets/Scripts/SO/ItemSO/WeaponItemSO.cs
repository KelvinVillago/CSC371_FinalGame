using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewWeaponItemSO", menuName = "Scribtable Objects / Item / New Weapon Item", order = 3)]
public class WeaponItemSO : ShopItemSO2
{
    [field: Header("Basic Weapon Item Properties")]
    [field: Tooltip("The upgrades avalible to this weapon")]
    [field: SerializeField] public List<WeaponUpgradeOS> WeaponUpgradeDB { get; internal set; }
}

