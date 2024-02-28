using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabaseSO", menuName = "Item Database SO / New Item Database OS", order = 1)]
public class ItemsDatabaseSO : ScriptableObject
{
    public List<WeaponItemSO> WeaponsDB;
    public List<DefenseItemSO> DefenseDB;
    public List<AnimalItemSO> AnimalDB;
    //public List<MscItemSO> ItemsDB;
}