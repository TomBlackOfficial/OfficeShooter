using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum SpawnMethod
{
    ColliderBoundaries,
    SpawnPoints
}

[System.Serializable]
public class EnemyWave
{
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public int maxEnemyCount = 10;
    public float spawnInterval = 1f;
    public float initialDelay = 0f;
}

public class EnemySpawner2 : MonoBehaviour
{
    public List<EnemyWave> waves = new List<EnemyWave>();
    public SpawnMethod spawnMethod = SpawnMethod.ColliderBoundaries;

    [Header("Spawn Method: Collider Boundaries")]
    public List<Collider2D> spawnColliders = new List<Collider2D>();

    [Header("Spawn Method: Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();

    private int currentWaveIndex = 0;
    private int currentEnemyCount = 0;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            EnemyWave currentWave = waves[currentWaveIndex];

            yield return new WaitForSeconds(currentWave.initialDelay);

            for (int i = 0; i < currentWave.maxEnemyCount; i++)
            {
                if (currentEnemyCount >= currentWave.maxEnemyCount)
                {
                    // No more enemies to spawn for this wave
                    break;
                }

                GameObject enemyPrefab = GetRandomEnemyPrefab(currentWave.enemyPrefabs);
                SpawnEnemy(enemyPrefab);

                currentEnemyCount++;

                yield return new WaitForSeconds(currentWave.spawnInterval);
            }

            currentWaveIndex++;
        }

        Debug.Log("All waves spawned!");
    }

    private GameObject GetRandomEnemyPrefab(List<GameObject> enemyPrefabs)
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (spawnMethod == SpawnMethod.ColliderBoundaries)
        {
            SpawnInColliderBoundaries(enemyPrefab);
        }
        else if (spawnMethod == SpawnMethod.SpawnPoints)
        {
            SpawnAtSpawnPoints(enemyPrefab);
        }
    }

    private void SpawnInColliderBoundaries(GameObject enemyPrefab)
    {
        if (spawnColliders.Count == 0)
        {
            Debug.LogError("No spawn colliders assigned for Collider Boundaries spawn method.");
            return;
        }

        Collider2D randomCollider = spawnColliders[Random.Range(0, spawnColliders.Count)];
        Vector3 spawnPosition = GetRandomPositionInCollider(randomCollider);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private void SpawnAtSpawnPoints(GameObject enemyPrefab)
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned for Spawn Points spawn method.");
            return;
        }

        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        Instantiate(enemyPrefab, randomSpawnPoint.position, Quaternion.identity);
    }

    private Vector3 GetRandomPositionInCollider(Collider2D collider)
    {
        Vector3 randomPoint = collider.bounds.center + new Vector3(
            Random.Range(-collider.bounds.extents.x, collider.bounds.extents.x),
            Random.Range(-collider.bounds.extents.y, collider.bounds.extents.y),
            0f
        );

        return randomPoint;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemySpawner2))]
public class EnemySpawnerEditor : Editor
{
    SerializedProperty wavesProperty;
    SerializedProperty spawnMethodProperty;
    SerializedProperty spawnCollidersProperty;
    SerializedProperty spawnPointsProperty;

    private void OnEnable()
    {
        wavesProperty = serializedObject.FindProperty("waves");
        spawnMethodProperty = serializedObject.FindProperty("spawnMethod");
        spawnCollidersProperty = serializedObject.FindProperty("spawnColliders");
        spawnPointsProperty = serializedObject.FindProperty("spawnPoints");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EnemySpawner2 spawner = (EnemySpawner2)target;

        // Display the number of waves
        EditorGUILayout.LabelField("Number of Waves:", spawner.waves.Count.ToString());

        // Add a button to add a new wave
        if (GUILayout.Button("Add Wave"))
        {
            spawner.waves.Add(new EnemyWave());
        }

        // Display spawn method
        EditorGUILayout.PropertyField(spawnMethodProperty);

        if (spawner.spawnMethod == SpawnMethod.ColliderBoundaries)
        {
            // Display collider boundaries list
            EditorGUILayout.PropertyField(spawnCollidersProperty, true);
        }
        else if (spawner.spawnMethod == SpawnMethod.SpawnPoints)
        {
            // Display spawn points list
            EditorGUILayout.PropertyField(spawnPointsProperty, true);
        }

        // Display wave properties
        for (int i = 0; i < spawner.waves.Count; i++)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Wave " + (i + 1));

            SerializedProperty waveProperty = wavesProperty.GetArrayElementAtIndex(i);
            SerializedProperty enemyPrefabsProperty = waveProperty.FindPropertyRelative("enemyPrefabs");
            SerializedProperty maxEnemyCountProperty = waveProperty.FindPropertyRelative("maxEnemyCount");
            SerializedProperty spawnIntervalProperty = waveProperty.FindPropertyRelative("spawnInterval");
            SerializedProperty initialDelayProperty = waveProperty.FindPropertyRelative("initialDelay");

            EditorGUILayout.PropertyField(enemyPrefabsProperty, true);
            EditorGUILayout.PropertyField(maxEnemyCountProperty);
            EditorGUILayout.PropertyField(spawnIntervalProperty);
            EditorGUILayout.PropertyField(initialDelayProperty);

            // Add a button to remove the wave
            if (GUILayout.Button("Remove Wave"))
            {
                spawner.waves.RemoveAt(i);
                break;
            }

            EditorGUILayout.Space();
        }

        // Apply changes
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
