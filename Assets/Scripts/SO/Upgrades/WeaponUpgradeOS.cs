using System;
using UnityEngine;


public abstract class WeaponUpgradeOS : ScriptableObject
{
    public abstract void Apply(GameObject target);
}

[Serializable]
[CreateAssetMenu(fileName = "newProjectileWeaponUpgradeOS", menuName = "Scribtable Objects / WeaponUpgradeOS / Guns / New Gun Upgrade", order = 1)]
public class ProjectileWeaponUpgradeOS : WeaponUpgradeOS
{
    public override void Apply(GameObject target)
    {
        //All the gun properties go here....
    }
}

[Serializable]
[CreateAssetMenu(fileName = "newBulletUpgradeOS", menuName = "Scribtable Objects / WeaponUpgradeOS / Guns / new Bullet Upgrade", order = 2)]
public class BulletUpgradeOS: WeaponUpgradeOS
{
    public float speed;
    public Color color;
    public override void Apply(GameObject target)
    {
        //Target is the bullet
        //change the color
        if (color != null)
        {
            target.GetComponent<Renderer>().material.color = color;
        }
        //Increase or Decrease the speed of the bullet
        target.GetComponent<Bullet>().bulletSpeed = speed;
    }
}

[Serializable]
[CreateAssetMenu(fileName = "newSwordUpgradeOS", menuName = "Scribtable Objects / WeaponUpgradeOS / New Sword Upgrade", order = 2)]
public class SwordUpgradeOS : WeaponUpgradeOS
{
    //This can be cahanged to any non projectile weapon?
    public override void Apply(GameObject target)
    {
        //All the changable properties go here....
    }
}

