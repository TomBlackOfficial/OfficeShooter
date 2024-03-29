using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Data/New Weapon", fileName = "New Weapon")]
public class WeaponData : ScriptableObject
{
    public Sprite weaponSprite;
    public ProjectileData projectileData;
    public float fireRate;
    public bool overrideDamage = false;
    public int damage = 1;
}
