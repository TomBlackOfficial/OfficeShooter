using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable
{
    protected AdvancedEnemyAI ai;
    protected Rigidbody2D rb;
    protected Animator anim;

    protected float moveSpeedMultiplier = 1;

    protected virtual void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ai = GetComponent<AdvancedEnemyAI>();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);

        PlayCustomAnimation("GetHit");
    }

    protected override void Die()
    {
        base.Die();

        DestroyGameObject();
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
