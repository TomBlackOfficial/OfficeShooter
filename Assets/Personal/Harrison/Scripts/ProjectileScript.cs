using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private ProjectileData _projectileData;
    private Rigidbody2D rb;
    private float _forwardAngle;
    private float _timeAlive;
    private bool _belongsToPlayer = true;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
        rb = GetComponent<Rigidbody2D>();
        _timeAlive = 0;
        rb.gravityScale = 0;
    }

    private void Update()
    {
        if (_projectileData != null)
        {
            rb.velocity = transform.up * _projectileData.projectileSpeed;
        }
        if (_timeAlive <= 0)
        {
            ProjectilePoolScript.INSTANCE.FreeProjectile(gameObject);
        }
        _timeAlive -= Time.deltaTime;
    }

    public void InitializeProjectile(Vector3 position, Vector3 forward, ProjectileData data, bool playerProjectile)
    {
        transform.position = position;
        transform.up = forward;
        SetProjectileData(data);
        _belongsToPlayer = playerProjectile;
    }

    private void SetProjectileData(ProjectileData data)
    {
        if (data != null)
        {
            _projectileData = data;
            _timeAlive = _projectileData.projectileRange / _projectileData.projectileSpeed;
            if (_projectileData.projectileSprite != null)
            {
                GetComponent<SpriteRenderer>().sprite = _projectileData.projectileSprite;
            }
        }
    }
}
