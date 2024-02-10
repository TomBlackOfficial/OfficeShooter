using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdvancedEnemyAI : BasicEnemyAI
{
    [Tooltip("Time in seconds between attacks when within radius (resets when out of radius)")]
    [SerializeField] protected float attackDelay = 5f;
    [Tooltip("Radius which enemy must be within to attack the target")]
    [SerializeField] protected float attackRadius = 5f;
    [Tooltip("Distance from the target the enemy will stop when moving in for attack")]
    [SerializeField] protected float attackStopDistance = 1f;
    [Tooltip("Move speed when moving in for an attack")]
    [SerializeField] protected float attackMoveSpeed = 4f;
    [Tooltip("Spawn a visual effect prefab when attacking?")]
    [SerializeField] protected bool useAttackVFX = false;
    [Tooltip("Visual effect prefab spawned in front of enemy when attack is performed")]
    [SerializeField] private GameObject attackVFX;

    [Tooltip("Name of animation clip to be played when enemy attacks (case sensitive)")]
    [SerializeField] private string attackAnimationName;

    [Tooltip("Custom events to call when enemy performs an attack")]
    [SerializeField] private UnityEvent attackEvents;

    private Vector2 lastTargetPos;
    protected Vector2 directionToLastPos;

    private float distanceToLastPos = 0;
    private float attackTimer;

    private bool isAttacking;

    protected override void Start()
    {
        base.Start();

        attackTimer = Random.Range(0f, attackDelay / 2f);
    }

    protected override void Update()
    {
        base.Update();

        AttackingInput();
    }

    protected override void MovementUpdate()
    {
        if (isDead)
            return;

        if (!isAttacking)
        {
            desiredVelocity = GetPursueVelocity(target, rb, stopDistance, pursuit_deadzone);

            if (retreat.factor == 1)
            {
                desiredVelocity *= moveSpeed;
            }
            else
            {
                desiredVelocity *= retreatMoveSpeed;
            }
        }
        else if (isAttacking && distanceToLastPos > attackStopDistance)
        {
            desiredVelocity = directionToLastPos * attackMoveSpeed;
        }

        Move();

        if (rb.velocity.magnitude > 0.01f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    private void AttackingInput()
    {
        if (isDead)
            return;

        distanceToLastPos = (lastTargetPos - rb.position).magnitude;
        directionToLastPos = lastTargetPos - rb.position;

        if (distance > attackRadius)
        {
            attackTimer = 0;
            return;
        }

        if (isAttacking)
        {
            if (distanceToLastPos <= attackStopDistance + 0.1f || distance <= attackStopDistance + 0.1f)
            {
                Attack();

                isAttacking = false;
            }
        }

        if (!isAttacking)
        {
            attackTimer += Time.deltaTime;
        }

        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            StartAttack();
        }
    }

    private void StartAttack()
    {
        if (target == null)
            return;

        lastTargetPos = target.transform.position;
        isAttacking = true;
    }

    private void Attack()
    {
        if (anim != null && useAnimation)
            anim.Play(attackAnimationName);

        if (attackVFX != null && useAttackVFX)
        {
            GameObject newObject = Instantiate(attackVFX, transform.position, Quaternion.identity);
            newObject.transform.right = directionToTarget;
            newObject.transform.parent = transform;
            newObject.transform.position += newObject.transform.right * 0.3f;

            Destroy(newObject, 0.5f);
        }

        attackEvents.Invoke();
    }

    public bool GetIsAttacking() { return isAttacking; }
}
