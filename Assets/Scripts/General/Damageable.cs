using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] protected GameObject deathVFX;
    [SerializeField] protected GameObject lootDrop;

    [Header("Basic Info")]
    [SerializeField] protected int maxHealth;

    protected int currentHealth;
    protected bool dead = false;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int amount = 1)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        if (gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (deathVFX != null)
            {
                GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
                Destroy(vfx, 1f);
            }
            LootPoolManager.INSTANCE.CreateLoot(transform.position);
        }
        dead = true;
    }

    public bool IsDead() { return dead; }
}
