using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Transform _projectileSpawnPoint;
    private bool _attachedToPlayer = false;
    private bool _firing = false;
    private float _fireTime = 0;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        if (transform.parent.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            _attachedToPlayer = true;
        }
        else
        {
            _attachedToPlayer = false;
        }
        SwapWeapon(_weaponData);
    }
    public void SetTargetPosition(Vector2 target)
    {
        transform.up = new Vector3(target.x, target.y, 0);
    }
    public void SetTargetPosition(Vector3 target)
    {
        transform.up = new Vector3(target.x, target.y, 0);
    }
    public void StartFiringProjectiles()
    {
        _firing = true;
    }
    public void StopFiringProjectiles()
    {
        _firing = false;
    }
    private void Update()
    {
        _fireTime -= Time.deltaTime;
        if (_fireTime <= 0 && _firing)
        {
            _fireTime = _weaponData.fireRate;
            GameObject projectile = ProjectilePoolScript.INSTANCE.UseProjectile();
            projectile.SetActive(true);
            ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
            projectileScript.InitializeProjectile(_projectileSpawnPoint.position, transform.up, _weaponData.projectileData, _attachedToPlayer);
        }
    }
    public void SwapWeapon(WeaponData data)
    {
        _weaponData = data;
        _spriteRenderer.sprite = _weaponData.weaponSprite;
    }
}
