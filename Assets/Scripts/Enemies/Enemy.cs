using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected AdvancedEnemyAI ai;
    protected Rigidbody2D rb;
    protected Animator anim;

    [Header("Assign")]
    [SerializeField] protected GameObject deathVFX;
    [SerializeField] protected GameObject lootDrop;

    [Header("Basic Info")]
    [SerializeField] protected int health;

    protected float moveSpeedMultiplier = 1;

    [HideInInspector] public bool dead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ai = GetComponent<AdvancedEnemyAI>();
    }

    public void TakeDamage(int amount)
    {
        if (health <= 0)
            return;

        health -= amount;

        PlayCustomAnimation("GetHit");

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        if (deathVFX != null)
        {
            GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(vfx, 1);
        }
        if (lootDrop != null)
        {
            GameObject loot = Instantiate(lootDrop, transform.position, Quaternion.identity);
        }

        PlayCustomAnimation("Die");
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        dead = true;
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void SetMoveSpeedMultipler(float value)
    {
        ai.SetMoveSpeedMultiplier(value);
    }

    public void PlayCustomAnimation(string clipName)
    {
        if (dead)
            return;

        anim.Play(clipName);
    }

    public void SetAnimationBool(string boolName, bool value)
    {
        if (dead)
            return;

        anim.SetBool(boolName, value);
    }
}
