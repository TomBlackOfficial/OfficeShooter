using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private ProjectileDataScript _projectileData;
    private Rigidbody2D rb;
    private float _forwardAngle;
    private float _timeAlive;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
        rb = GetComponent<Rigidbody2D>();
        Vector3 forward = new Vector3(-1, 0, 0);
        _timeAlive = 0;
        InitializeProjectile(forward, _projectileData);
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
            ProjectilePoolScript.INSTANCE
        }
        _timeAlive -= Time.deltaTime;
    }

    public void InitializeProjectile(Vector3 forward, ProjectileDataScript data)
    {
        transform.up = forward;
        SetProjectileData(data);
    }

    private void SetProjectileData(ProjectileDataScript data)
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
