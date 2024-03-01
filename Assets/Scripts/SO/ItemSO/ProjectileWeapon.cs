using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "NewProjectileWeaponOS", menuName = "Scribtable Objects / Item / New Projectile Weapon Item", order = 4)]
public class ProjectileWeaponOS : WeaponItemSO
{
    [field: Header("Projectile Weapon Properties")]
    [field: SerializeField] public List<BulletUpgradeOS> bulletUpgradeDB { get; private set; }
}
