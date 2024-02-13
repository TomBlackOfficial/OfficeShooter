using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Transform _projectileSpawnPoint;

    public void SetTargetPosition(Vector2 target)
    {
        transform.up = new Vector3(target.x, target.y, 0);
    }
    public void SetTargetPosition(Vector3 target)
    {
        transform.up = new Vector3(target.x, target.y, 0);
    }
    public void FireProjectile()
    {

    }
}
