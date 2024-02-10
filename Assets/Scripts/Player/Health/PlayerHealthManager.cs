using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int maxHealth = 6;

    [Header("Assigned Variables")]
    //[SerializeField] 

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
    }

    public int GetCurrentHealth() { return currentHealth; }
}
