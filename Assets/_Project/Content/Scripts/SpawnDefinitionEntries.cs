using System;
using UnityEngine;

[Serializable]
public class ItemSpawnEntry
{
    [SerializeField] private ItemDefinition definition;
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] [Range(0f, 1f)] private float spawnChance = 1f;
    [SerializeField] private int weight = 1;
    [SerializeField] private int maxCount = 1;

    public ItemDefinition Definition => definition;
    public GameObject PickupPrefab => pickupPrefab;
    public float SpawnChance => spawnChance;
    public int Weight => Mathf.Max(1, weight);
    public int MaxCount => Mathf.Max(1, maxCount);
}

[Serializable]
public class EnemySpawnEntry
{
    [SerializeField] private EnemyDefinition definition;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] [Range(0f, 1f)] private float spawnChance = 1f;
    [SerializeField] private int weight = 1;
    [SerializeField] private int maxCount = 1;

    public EnemyDefinition Definition => definition;
    public GameObject EnemyPrefab => enemyPrefab;
    public float SpawnChance => spawnChance;
    public int Weight => Mathf.Max(1, weight);
    public int MaxCount => Mathf.Max(1, maxCount);
}
