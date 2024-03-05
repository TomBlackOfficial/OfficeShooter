using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddLootScript : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            LootPoolManager.INSTANCE.CreateLoot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
