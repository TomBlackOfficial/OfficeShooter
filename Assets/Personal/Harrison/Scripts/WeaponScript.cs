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
    private float _fireRateDividend = 10;
    private float _fireRateDivisor;
    private float _minFireTime = 0.1f;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SwapWeapon(_weaponData);

        //Calculate divisor for fire rate
        _fireRateDivisor = _fireRateDividend / _minFireTime;
    }

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
    }
    public void SetTargetPosition(Vector2 target)
    {
        transform.right = new Vector3(target.x, target.y, 0);
    }
    public void SetTargetPosition(Vector3 target)
    {
        transform.right = new Vector3(target.x, target.y, 0);
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
            //Calculating the _fireCooldown based on the firerate
            _fireTime = _fireRateDividend / (Mathf.Clamp(Mathf.Log(_weaponData.fireRate / 10), 0.05f, 1f) * _fireRateDivisor);
            Debug.Log(_fireTime.ToString());
            //_fireTime = _minFireTime;
            GameObject projectile = ProjectilePoolScript.INSTANCE.UseProjectile();
            projectile.SetActive(true);
            ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
            projectileScript.InitializeProjectile(_projectileSpawnPoint.position, transform.right, _weaponData.projectileData, _attachedToPlayer);
        }
    }
    public void SwapWeapon(WeaponData data)
    {
        _weaponData = data;
        _spriteRenderer.sprite = _weaponData.weaponSprite;
    }
}
