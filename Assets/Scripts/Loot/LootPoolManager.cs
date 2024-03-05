using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class LootPoolManager : MonoBehaviour
{
    private Queue<GameObject> _lootPool = new Queue<GameObject>();
    [SerializeField] private int _initialPoolSize;
    [SerializeField] private int _maxPoolSize;
    [SerializeField] private GameObject _baseLootPrefab;
    [SerializeField] private LootTableData _lootTable;
    private int _currentPoolSize = 0;
    public static LootPoolManager INSTANCE;

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
            GameObject loot = Instantiate(_baseLootPrefab, transform);
            loot.SetActive(false);
            _lootPool.Enqueue(loot);
        }
        _currentPoolSize = _lootPool.Count;
    }

    public void FreeLoot(GameObject loot)
    {
        loot.SetActive(false);
        _lootPool.Enqueue(loot);
    }

    public void CreateLoot(Vector3 position)
    {
        List<LootData> lootList = _lootTable.lootAndDropChance.Keys.ToList();
        lootList = lootList.OrderBy(x => Random.value).ToList();

        LootData selectedLoot = null;

        for (int l = 0; l < lootList.Count; l++)
        {
            if (Random.Range(0f,100f) < _lootTable.lootAndDropChance[lootList[l]])
            {
                selectedLoot = lootList[l];
                break;
            }
        }

        if (selectedLoot == null)
        {
            return;
        }

        GameObject lootDrop = null;

        if (_lootPool.Count > 0)
        {
            lootDrop = _lootPool.Dequeue();
            lootDrop.transform.position = position;
        }
        else if (_currentPoolSize < _maxPoolSize)
        {
            _currentPoolSize++;
            lootDrop = Instantiate(_baseLootPrefab, position, Quaternion.identity, transform);
        }
        if (lootDrop == null)
        {
            return;
        }
        lootDrop.GetComponent<LootDropScript>().SetLootData(selectedLoot);
        lootDrop.SetActive(true);
    }

    public void CreateLoot(Vector3 position, WeaponData weapon)
    {
        List<LootData> lootList = _lootTable.lootAndDropChance.Keys.ToList();

        LootData selectedLoot = null;

        for (int l = 0; l < lootList.Count; l++)
        {
            if (lootList[l].weaponData == weapon)
            {
                selectedLoot = lootList[l];
                break;
            }
        }

        if (selectedLoot == null)
        {
            return;
        }

        GameObject lootDrop = null;

        if (_lootPool.Count > 0)
        {
            lootDrop = _lootPool.Dequeue();
            lootDrop.transform.position = position;
        }
        else if (_currentPoolSize < _maxPoolSize)
        {
            _currentPoolSize++;
            lootDrop = Instantiate(_baseLootPrefab, position, Quaternion.identity, transform);
        }
        if (lootDrop == null)
        {
            return;
        }
        lootDrop.GetComponent<LootDropScript>().SetLootData(selectedLoot);
        lootDrop.SetActive(true);
    }
}
