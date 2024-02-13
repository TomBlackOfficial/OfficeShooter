using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolScript : MonoBehaviour
{
    private Queue<GameObject> _projectilePool;
    [SerializeField] private int _initialPoolSize;
    [SerializeField] private int _maxPoolSize;
    [SerializeField] private GameObject _baseProjectilePrefab;
    private int _currentPoolSize = 0;
    public static ProjectilePoolScript INSTANCE;

    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(gameObject);
            return;
        }
        INSTANCE = this;

    }

    private void Start()
    {
        CreateInitialPool();
    }

    private void CreateInitialPool()
    {
        for (int p = 0; p < _initialPoolSize; p++)
        {
            GameObject projectile = Instantiate(_baseProjectilePrefab, transform);
            projectile.SetActive(false);
            _projectilePool.Enqueue(projectile);
        }
        _currentPoolSize = _projectilePool.Count;
    }

    private void FreeProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        _projectilePool.Enqueue(projectile);
    }

    private void UseProjectile()
    {
        if (_projectilePool.Count > 0)
        {

        }
    }
}
