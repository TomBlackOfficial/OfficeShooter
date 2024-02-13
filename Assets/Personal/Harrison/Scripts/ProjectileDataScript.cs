using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Data/New Projectile", fileName = "New Projectile")]
public class ProjectileDataScript : ScriptableObject
{
    public Sprite projectileSprite;
    public float projectileSpeed = 1;
    public float projectileRange = 8;
}
