using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot Data/New Loot Drop", fileName = "NewLootDrop")]
public class LootData : ScriptableObject
{
    public Sprite lootSprite;
    public bool weaponDrop;
    public WeaponData weaponData;
    public bool healthDrop;
    public int healAmount;
}
