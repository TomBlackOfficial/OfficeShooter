using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropScript : MonoBehaviour
{
    private LootData _myLootData;

    private void OnEnable()
    {
        if (_myLootData != null)
        {

        }
    }

    public void SetLootData(LootData data)
    {
        if (data == null)
        {
            return;
        }
        _myLootData = data;
    }
}
