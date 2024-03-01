using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDatabaseSO", menuName = "Scribtable Objects / Database/ New Item DB", order = 1)]
public class ItemsDatabaseSO : ScriptableObject
{
    public List<WeaponItemSO> WeaponsDB;
    public List<DefenseItemSO> DefenseDB;
    public List<AnimalItemSO> AnimalDB;
    //public List<MscItemSO> ItemsDB;
}