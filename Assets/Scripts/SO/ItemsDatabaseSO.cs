using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDatabaseSO", menuName = "Scribtable Objects / Database/ New Item DB", order = 1)]
public class ItemsDatabaseSO : ScriptableObject
{
    [Header("All Weapons")]
    public List<WeaponItemSO> WeaponsDB;
    [Header("All Defelse Items")]
    public List<DefenseItemSO> DefenseDB;
    [Header("All Animal Items")]
    public List<AnimalItemSO> AnimalDB;
    [Header("Refrences to all other items: Anything of type ItemSO")]
    public List<ItemSO> MscItemsDB;
}