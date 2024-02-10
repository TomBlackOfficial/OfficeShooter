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
            if (_enableDebugMode)
            {
                Debug.Log("Start Firing");
            }
        }
        if (context.canceled)
        {
            if (_enableDebugMode)
            {
                Debug.Log("Stop Firing");
            }
        }
    }

    private void Awake()
    {
        INSTANCE = this;

        rb = GetComponent<Rigidbody2D>();
        healthManager = GetComponent<PlayerHealthManager>();

        _defaultColour = sprite.color;
    }

    private void Start()
    {
        ChangeState(PlayerState.Default);
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

    public bool GetVulnerability()
    {
        return !_invulnerable;
    }
}
