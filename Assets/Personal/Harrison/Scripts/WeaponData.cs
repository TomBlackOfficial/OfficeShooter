using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Data/New Weapon", fileName = "New Weapon")]
public class WeaponData : ScriptableObject
{
    public Sprite weaponSprite;
    public ProjectileData projectileData;
}
