using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Melee Stats")]
    [SerializeField] protected int damage;

    public void Attack()
    {
        if (ai.GetDistance() < 2f)
        {
            //PlayerController.INSTANCE.TakeDamage(damage);
        }
    }
}
