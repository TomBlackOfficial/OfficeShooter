using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropScript : MonoBehaviour
{
    private LootData _myLootData;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _spriteGameObject;
    [SerializeField] private Sprite _pickupKeySprite;
    [SerializeField] private float _keyDistance = 0.5f;
    [SerializeField] private GameObject _displayGameObject;
    private Vector2 _direction;
    private float _force;
    private bool _rotateClockwise;
    private float _rotation;
    [SerializeField] private Vector2 _minMaxInitialForce;
    [SerializeField] private float _slowForceFactor = 0.04f;
    [SerializeField] private float _slowRotationFactor = 0.07f;
    [SerializeField] private Vector2 _minMaxInitialRotationSpeed;
    

    private void Awake()
    {
        if (_displayGameObject != null)
        {
            if (_displayGameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
            {
                sr.sprite = _pickupKeySprite;
                _displayGameObject.transform.position = new Vector3(transform.position.x, transform.position.y + _keyDistance, transform.position.z);
            }
        }
    }

    private void OnEnable()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        _direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        _force = Random.Range(_minMaxInitialForce.x, _minMaxInitialForce.y);
        if (_myLootData.rotateOnSpawn)
        {
            _rotation = Random.Range(_minMaxInitialRotationSpeed.x, _minMaxInitialRotationSpeed.y);
            _rotateClockwise = (Random.Range(0, 2) == 0);
        }
        else
        {
            _spriteGameObject.transform.rotation.SetEulerAngles(0, 0, 0);
        }
        if (_myLootData != null)
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.sprite = _myLootData.lootSprite;
            }
        }
        ShowButton(false);
    }

    private void Update()
    {
        if (_spriteRenderer != null)
        {
            if (_spriteRenderer.sprite != _myLootData.lootSprite)
            {
                _spriteRenderer.sprite = _myLootData.lootSprite;
            }
        }
        if (_force > 0)
        {
            Vector3 newPosition = transform.position + (new Vector3(_direction.x, _direction.y, 0) * _force * Time.deltaTime);
            transform.position = newPosition;
            _force = Mathf.Lerp(_force, 0, _slowForceFactor);
            if (_force < 0.0001f)
            {
                _force = 0f;
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            _spriteGameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        if (_rotation > 0 && _myLootData.rotateOnSpawn)
        {
            float rotation = _rotation;
            if (_rotateClockwise)
            {
                rotation *= -1;
            }
            _spriteGameObject.transform.Rotate(Vector3.forward, rotation);
            _rotation = Mathf.Lerp(_rotation, 0, _slowRotationFactor);
            if (_rotation < 0.005f)
            {
                _rotation = 0f;
            }
        }
    }

    public void SetLootData(LootData data)
    {
        if (data == null)
        {
            return;
        }
        _myLootData = data;
    }

    public LootData UseLoot()
    {
        return _myLootData;
    }

    public void ShowButton(bool show)
    {
        _displayGameObject.SetActive(show);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            Debug.Log("Player Detected");
            player.AddLoot(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            Debug.Log("Player Removed");
            ShowButton(false);
            player.RemoveLoot(this);
        }
    }
}
