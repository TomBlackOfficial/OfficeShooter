using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicEnemyAI : CustomEnemyAI
{
    protected Rigidbody2D rb;

    [Tooltip("Enemy sprite, used to flip sprite towards target")]
    [SerializeField] protected SpriteRenderer sprite;
    [Tooltip("Flip the sprite to face the movement direction")]
    [SerializeField] protected bool flipSprite = true;

    [Tooltip("Object for enemy to move towards, eg. Player")]
    [SerializeField] protected GameObject target;
    [Tooltip("Do you want to automatically find the nearest target?")]
    [SerializeField] protected bool autoTargeting = true;
    [Tooltip("The target layer to look for within the target radius")]
    [SerializeField] protected LayerMask targetLayerMask;
    [Tooltip("The radius in which the target must be to lock on")]
    [SerializeField] protected float targetRadius = 8;

    [Tooltip("Does your enemy use animation?")]
    [SerializeField] protected bool useAnimation = true;
    [Tooltip("The animator component attached to your enemy")]
    [SerializeField] protected Animator anim;
    [Tooltip("Does your enemy use animation?")]
    [SerializeField] protected string movingBoolName;

    [Tooltip("The movement speed when moving towards the player or strafing")]
    [SerializeField] protected float moveSpeed = 2.75f;
    [Tooltip("The movement speed when moving away from the player")]
    [SerializeField] protected float retreatMoveSpeed = 3f;
    [Tooltip("The movement speed when moving towards the player or strafing")]
    [Range(0f, 1f)] [SerializeField] protected float movementSmoothing = 0.3f;
    

    protected Vector2 velocity;
    protected Vector2 desiredVelocity;
    protected Vector2 directionToTarget;

    protected float distance;
    protected float moveSpeedMultiplier = 1;

    protected bool isMoving = false;
    protected bool isDead = false;

    protected void Reset()
    {
        if (anim == null)
        {
            Animator autoAnim = GetComponent<Animator>();
            if (autoAnim != null)
                anim = autoAnim;
        }
        
        if (sprite == null)
        {
            SpriteRenderer autoSprite = GetComponent<SpriteRenderer>();
            if (autoSprite != null)
                sprite = autoSprite;
        }
        
        if (rb == null)
        {
            Rigidbody2D autoRb = GetComponent<Rigidbody2D>();
            if (autoRb != null)
            {
                rb = autoRb;
                rb.gravityScale = 0;
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (rb.gravityScale != 0)
            rb.gravityScale = 0;
    }

    protected virtual void Update()
    {
        if (isDead)
        {
            return;
        }

        if (target == null && autoTargeting)
        {
            GetNearestTarget();
        }

        if (anim != null && useAnimation)
        {
            UpdateAnimation();
        }

        if (target == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        distance = ((Vector2)target.transform.position - rb.position).magnitude;
        directionToTarget = target.transform.position - transform.position;

        if (directionToTarget.x < 0 && sprite != null && flipSprite)
        {
            sprite.flipX = true;
        }
        else if (directionToTarget.x > 0 && sprite != null && flipSprite)
        {
            sprite.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        if (isDead || target == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }
            

        MovementUpdate();
    }

    protected void GetNearestTarget()
    {
        Collider2D[] nearbyTargets = Physics2D.OverlapCircleAll(transform.position, targetRadius, targetLayerMask);

        if (nearbyTargets.Length <= 0)
            return;

        GameObject closestTarget = nearbyTargets[0].transform.gameObject;
        float dist = Vector2.Distance(transform.position, nearbyTargets[0].transform.position);
        
        if (nearbyTargets.Length > 1)
        {
            foreach (Collider2D currentTarget in nearbyTargets)
            {
                var tempDist = Vector2.Distance(transform.position, currentTarget.transform.position);
                if (tempDist < dist)
                {
                    closestTarget = currentTarget.transform.gameObject;
                }
            }
        }

        target = closestTarget;
    }

    protected virtual void MovementUpdate()
    {
        desiredVelocity = GetPursueVelocity(target, rb, stopDistance, pursuit_deadzone);

        if (retreat.factor == 1)
        {
            desiredVelocity *= retreatMoveSpeed;
        }
        else
        {
            desiredVelocity *= moveSpeed;
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

    protected void Move()
    {
        desiredVelocity *= moveSpeedMultiplier;
        velocity = Vector2.Lerp(rb.velocity, desiredVelocity, 1.1f - movementSmoothing);
        rb.velocity = velocity;
    }

    private void UpdateAnimation()
    {
        anim.SetBool(movingBoolName, isMoving);
    }

    public void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
    }

    public void SetMoveSpeedMultiplier(float multiplier) { moveSpeedMultiplier = multiplier; }
    public float GetDistance() { return distance; }
    public bool GetIsMoving() { return isMoving; }
    public bool GetIsDead() { return isDead; }
    public Vector2 GetVelocity() { return velocity; }
    public Vector2 GetDirectionToTarget() { return directionToTarget; }
    public Rigidbody2D GetRigidbody() { return rb; }
    public GameObject GetTarget() { return target; }
}
