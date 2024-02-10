using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CustomEnemyAI : MonoBehaviour
{
    [Tooltip("Draw debug rays visualizing the AI movement?")]
    [SerializeField] protected bool drawDebugRays = false;

    [Tooltip("Distance to stop from target and start strafing")]
    [SerializeField] protected float stopDistance = 4f;

    [Tooltip("Radius to check for objects to avoid")]
    [Range(0.1f, 2f)] [SerializeField] protected float collisionRadius = 0.7f;

    protected AIContext seek = new AIContext();
    protected AIContext strafe = new AIContext();
    protected AIContext retreat = new AIContext();
    protected AIContext bias = new AIContext();
    protected AIContext seperation = new AIContext();

    protected AIContext weights = new AIContext();

    protected List<AIContext> contexts = new List<AIContext>();

    protected Vector2 dir;
        
    protected float pursuit_deadzone;

    protected virtual void Start()
    {
        contexts = new List<AIContext> { seek, strafe, retreat, bias, seperation};

        pursuit_deadzone = Random.Range(0.2f, 0.4f);

        //Remove this line of code if you don't want the Stop Distance to be slightly randomized
        stopDistance = Random.Range(stopDistance - 0.5f, stopDistance + 0.5f);
    }

    protected virtual Vector2 GetPursueVelocity(GameObject target, Rigidbody2D rb, float arrivalDistance, float arrivalDeadzone)
    {
        Vector2 displacement = target.transform.position - transform.position;
        float distance = displacement.magnitude;

        seek = GetDirectionContext(displacement, seek);
        strafe = GetStrafeContext(displacement);
        bias = GetDirectionContext(rb.velocity.normalized, bias);

        retreat = GetRetreatContext(target, 100f);

        var neighbors = new List<GameObject>();
        var filter = new ContactFilter2D();
        filter.NoFilter();

        var cols = Physics2D.CircleCastAll(transform.position, collisionRadius, Vector2.zero);
        foreach (var c in cols)
        {
            neighbors.Add(c.transform.gameObject);
        }

        seperation = GetSeperationContext(neighbors, target.GetComponent<Rigidbody2D>(), 32);

        seek.factor = 1.0f;
        if (arrivalDistance - arrivalDeadzone != 0)
        {
            seek.factor = AIContext.MapValue(distance, arrivalDeadzone, arrivalDistance);
        }
        strafe.factor = 1.0f - seek.factor;

        bias.factor = 0.05f;

        if (distance < (stopDistance / 2f))
        {
            retreat.factor = 1;
            strafe.factor = 0;
        }
        else
        {
            retreat.factor = 0;
        }

        weights.Combine(contexts);

        dir = weights.GetDesiredDirection();

        if (drawDebugRays)
            DrawDebugRays();

        return dir;
    }

    private void DrawDebugRays()
    {
        float angle = 0;

        for (int i = 0; i < weights.size; i++)
        {
            Vector2 dir = (Quaternion.Euler(0, 0, angle) * Vector2.right);
            float currentWeight = weights.weights[i];

            if (currentWeight == weights.weights.Max())
            {
                Debug.DrawRay(transform.position, dir * 0.8f, Color.yellow);
            }
            else if (currentWeight > 0.5f)
            {
                Debug.DrawRay(transform.position, dir * 0.6f, Color.blue);
            }
            else
            {
                Debug.DrawRay(transform.position, dir * 0.4f, Color.red);
            }

            angle += 22.5f;
        }
    }

    public AIContext GetDirectionContext(Vector2 displacement, AIContext context)
    {
        if (displacement == Vector2.zero)
        {
            context.Clear();
            return context;
        }

        for (int i = 0; i < context.size; i++)
        {
            var value = Mathf.Cos(Vector2.SignedAngle(Vector2.right, displacement) * Mathf.Deg2Rad - context.GetAngle(i));
            var weight = (value * 0.5f + 0.5f);
            context.SetWeight(i, weight);
        }

        return context;

    }
    public AIContext GetStrafeContext(Vector2 displacement)
    {

        for (int i = 0; i < strafe.size; i++)
        {
            var value = Mathf.Cos(Vector2.SignedAngle(Vector2.right, displacement) * Mathf.Deg2Rad - strafe.GetAngle(i));
            var weight = 1.0f - Mathf.Abs(value + 0.25f);
            strafe.SetWeight(i, weight);
        }

        return strafe;

    }

    public AIContext GetRetreatContext(GameObject entity, float seperation_distance = 32f)
    {
        retreat.Clear();

        var away_vector = new Vector2();
        var max_factor = 0.0f;

        var displacement = (Vector2)transform.position - (Vector2)entity.transform.position;
        var distance = displacement.magnitude;
        if (distance < seperation_distance)
        {
            var factor = 1.0f - (distance / seperation_distance);
            max_factor = Mathf.Max(factor, max_factor);

            away_vector += (displacement / distance) * factor;
        }

        if (away_vector != Vector2.zero)
        {
            for (int i = 0; i < retreat.size; i++)
            {
                var value = Mathf.Cos(Vector2.SignedAngle(Vector2.right, away_vector) * Mathf.Deg2Rad - retreat.GetAngle(i));
                //var weight = 1.0f - Mathf.Abs(value - 0.8f);
                retreat.SetWeight(i, value * max_factor);
            }
        }

        return retreat;
    }

    public AIContext GetSeperationContext(List<GameObject> entities, Rigidbody2D exception, float seperation_distance)
    {
        seperation.Clear();

        var away_vector = new Vector2();
        var max_factor = 0.0f;

        foreach (var entity in entities)
        {
            if (entity.gameObject == gameObject || entity == exception)
            {
                continue;
            }

            Vector2 displacement = transform.position - entity.transform.position;
            float distance = displacement.magnitude;
            if (distance < seperation_distance)
            {
                var factor = 1.0f - (distance / seperation_distance);
                max_factor = Mathf.Max(factor, max_factor);

                away_vector += (displacement / distance) * factor;
            }
        }

        if (away_vector != Vector2.zero)
        {
            seperation.factor = 1;
            for (int i = 0; i < seperation.size; i++)
            {
                var value = Mathf.Cos(Vector2.SignedAngle(Vector2.right, away_vector) * Mathf.Deg2Rad - seperation.GetAngle(i));
                var weight = 1.0f - Mathf.Abs(value - 0.65f);
                seperation.SetWeight(i, weight);
            }
        }
        else
        {
            seperation.factor = 0;
        }

        return seperation;
    }

    public Vector2 GetMoveDirection() { return dir; }
}
