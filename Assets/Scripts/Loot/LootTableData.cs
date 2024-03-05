using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(menuName = "Loot Data/New Loot Table", fileName = "LootTable")]
public class LootTableData : ScriptableObject
{
    public SerializedDictionary<LootData, float> lootAndDropChance;
}
