using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerHealthManager))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController INSTANCE;

    [Header("Assigned Variables")]
    [SerializeField] private SpriteRenderer sprite;

    [Header("Movement Settings")]
    [SerializeField] private float _playerSpeed = 2;

    [Header("Dodge Settings")]
    [SerializeField] private float _dodgeTimeStartup = 0.15f;
    [SerializeField] private float _dodgeTime = 0.75f;
    [SerializeField] private float _dodgeSpeed = 3.5f;

    [Header("Weapon Settings")]
    [SerializeField] private WeaponData _starterWeapon;
    [SerializeField] private int _maxWeaponsHeld = 3;

    [Header("Debug Settings")]
    [SerializeField] private bool _enableDebugMode;
    [SerializeField] private Color _dodgeColour;

    private Rigidbody2D rb;
    private PlayerHealthManager healthManager;

    private Color _defaultColour;

    private Vector2 _movementInput = Vector2.zero;
    private Vector2 _lastMovement = Vector2.one;

    private bool _dodge = false;
    private bool _invulnerable = false;
    private bool _freezeNewMovementInput = false;
    private WeaponScript _myWeapon;
    private List<WeaponData> _heldWeapons = new List<WeaponData>();
    private int _currentEquipedWeapon = 0;
    

    private enum PlayerState
    {
        Default,
        Dodging
    }

    private PlayerState currentState = PlayerState.Default;

    public void InputMovement(InputAction.CallbackContext context)
    {
        // Get movement input from player controls

        _movementInput = context.ReadValue<Vector2>();
        if (_movementInput != Vector2.zero && !_freezeNewMovementInput)
        {
            _lastMovement = _movementInput;
        }
        if (_enableDebugMode)
        {
            //Debug.Log(_movementInput);
        }
    }
    public void InputDodge(InputAction.CallbackContext context)
    {
        // Get dodge input from player controls

        if (_dodge)
        {
            return;
        }
        if (context.started)
        {
            if (_enableDebugMode)
            {
                Debug.Log("Dodge");
            }
            _dodge = true;
        }
    }
    public void InputFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _myWeapon.StartFiringProjectiles();
            if (_enableDebugMode)
            {
                Debug.Log("Start Firing");
            }
        }
        if (context.canceled)
        {
            _myWeapon.StopFiringProjectiles();
            if (_enableDebugMode)
            {
                Debug.Log("Stop Firing");
            }
        }
    }

    public void InputLook(InputAction.CallbackContext context)
    {
        if (_myWeapon != null)
        {
            if (context.control.name == "Keyboard and Mouse")
            {
                Vector3 mousePosition = context.ReadValue<Vector3>();
                Vector3 lookAxis = mousePosition - transform.position;
                _myWeapon.SetTargetPosition(lookAxis);
            }
            if (context.control.name == "Controller")
            {
                Vector2 controllerLookAxis = context.ReadValue<Vector2>();
                _myWeapon.SetTargetPosition(controllerLookAxis);
            }
        }
    }

    public void InputSwapWeapon(InputAction.CallbackContext context)
    {
        int direction = context.ReadValue<int>();
        ChangeWeapon(direction);
    }

    private void Awake()
    {
        INSTANCE = this;

        rb = GetComponent<Rigidbody2D>();
        healthManager = GetComponent<PlayerHealthManager>();

        _defaultColour = sprite.color;
        rb.gravityScale = 0;
        Cursor.lockState = CursorLockMode.None;
        _myWeapon = GetComponentInChildren<WeaponScript>();
    }

    private void Start()
    {
        
        ChangeState(PlayerState.Default);
        if (_myWeapon != null)
        {
            _myWeapon.StopFiringProjectiles();
            _myWeapon.SwapWeapon(_starterWeapon);
        }
    }

    private void ChangeState(PlayerState newState)
    {
        if (_enableDebugMode)
        {
            Debug.Log($"Exiting {currentState} and entering {newState}");
        }
        currentState = newState;
        switch (newState)
        {
            case PlayerState.Default:
                StartCoroutine(DefaultState());
                break;
            case PlayerState.Dodging:
                StartCoroutine(DodgeState());
                break;
        }
    }

    private IEnumerator DefaultState()
    {
        _invulnerable = false;
        yield return null;
        PlayerState newState = PlayerState.Default;

        while (newState == currentState)
        {
            Vector2 movement = new Vector2(_movementInput.x, _movementInput.y);
            rb.velocity = movement * _playerSpeed;
            _myWeapon.SetTargetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            yield return null;
            if (_dodge)
            {
                newState = PlayerState.Dodging;
            }
        }
        ChangeState(newState);
    }

    private IEnumerator DodgeState()
    {
        FreezeMovement();
        float dodgeTime = 0;
        bool startup = true;
        while (_dodge)
        {
            Vector2 movement = new Vector2(_lastMovement.x, _lastMovement.y);
            rb.velocity = movement * _dodgeSpeed;

            if (dodgeTime >= _dodgeTimeStartup && startup)
            {
                startup = false;
                EnableInvulnerability();
            }
            if (dodgeTime >= _dodgeTimeStartup + _dodgeTime)
            {
                DisableInvulnerability();
                _dodge = false;
            }

            yield return null;
            dodgeTime += Time.deltaTime;
        }

        ResetMovement();
        ChangeState(PlayerState.Default);
    }

    public void TakeDamage(int amount)
    {
        healthManager.AddHealth(-amount);
    }

    private void ResetMovement()
    {
        _freezeNewMovementInput = false;
        if (_movementInput != Vector2.zero)
        {
            _lastMovement = _movementInput;
        }
    }

    private void FreezeMovement()
    {
        _freezeNewMovementInput = true;
    }

    private void EnableInvulnerability()
    {
        _invulnerable = true;
        if (_enableDebugMode)
        {
            sprite.color = _dodgeColour;
        }
    }
    private void DisableInvulnerability()
    {
        _invulnerable = false;
        if (_enableDebugMode)
        {
            sprite.color = _defaultColour;
        }
    }

    private void AddWeapon(WeaponData weapon)
    {
        if (_heldWeapons.Count == 0)
        {
            _heldWeapons.Add(weapon);
            _currentEquipedWeapon = 0;
            return;
        }
        if (_heldWeapons.Contains(weapon))
        {
            return;
        }
        if (_heldWeapons.Count == _maxWeaponsHeld)
        {
            _heldWeapons.Remove(_heldWeapons[_currentEquipedWeapon]);
        }
        _heldWeapons.Add(weapon);
        _currentEquipedWeapon = _heldWeapons.Count - 1;
        SwapWeapon(_heldWeapons[_currentEquipedWeapon]);
    }

    private void ChangeWeapon(int direction = 1)
    {
        if (direction == 0)
        {
            return;
        }
        if (direction > 0)
        {
            _currentEquipedWeapon++;
            if (_currentEquipedWeapon >= _heldWeapons.Count)
            {
                _currentEquipedWeapon = 0;
            }
        }
        else
        {
            _currentEquipedWeapon--;
            if (_currentEquipedWeapon < 0)
            {
                _currentEquipedWeapon = _heldWeapons.Count - 1;
            }
        }
        SwapWeapon(_heldWeapons[_currentEquipedWeapon]);
    }

    private void SwapWeapon(WeaponData weaponData)
    {
        if (_myWeapon == null)
        {
            Debug.LogError("Player does not have a Weapon Object");
            return;
        }
        _myWeapon.SwapWeapon(weaponData);
    }

    public bool GetVulnerability()
    {
        return !_invulnerable;
    }
}
