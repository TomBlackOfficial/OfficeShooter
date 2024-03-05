using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropScript : MonoBehaviour
{
    private LootData _myLootData;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _pickupKeySprite;
    [SerializeField] private float _keyDistance = 0.5f;
    [SerializeField] private GameObject _displayGameObject;

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
        if (_myLootData != null)
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            if (_spriteRenderer != null)
            {
                _spriteRenderer.sprite = _myLootData.lootSprite;
                Debug.Log("applied sprite");
            }
        }
        ShowButton(false);
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
