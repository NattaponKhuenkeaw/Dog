using System.Collections.Generic;
using UnityEngine;

public class roomManag : MonoBehaviour
{
    [Header("Data-Driven Rooms")]
    [SerializeField] private RoomSearchDefinition[] roomDefinitions;

    [System.Serializable]
    public class SpawnableItem
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float spawnChance = 1f;
    }

    [System.Serializable]
    public class RoomData
    {
        public GameObject roomPrefab;
        public string empati;
        public SpawnableItem[] items;
    }

    public RoomData[] rooms;

    private void Start()
    {
        if (TrySpawnFromDefinitions())
        {
            return;
        }

        if (rooms == null || rooms.Length == 0)
        {
            Debug.LogWarning("No room definitions or legacy rooms are assigned.");
            return;
        }

        int randomIndex = Random.Range(0, rooms.Length);
        RoomData selectedRoom = rooms[randomIndex];
        if (selectedRoom.roomPrefab == null)
        {
            Debug.LogWarning("Selected legacy room does not have a prefab assigned.");
            return;
        }

        GameObject roomInstance = Instantiate(selectedRoom.roomPrefab, Vector3.zero, Quaternion.identity);
        Debug.Log($"Randomized room Empati: {selectedRoom.empati}");

        List<Transform> validSpawnPoints = CollectSpawnPoints(roomInstance);
        SpawnLegacyItems(selectedRoom, validSpawnPoints);
    }

    private bool TrySpawnFromDefinitions()
    {
        if (roomDefinitions == null || roomDefinitions.Length == 0)
        {
            return false;
        }

        List<RoomSearchDefinition> validDefinitions = new List<RoomSearchDefinition>();
        foreach (RoomSearchDefinition definition in roomDefinitions)
        {
            if (definition != null && definition.RoomPrefab != null)
            {
                validDefinitions.Add(definition);
            }
        }

        if (validDefinitions.Count == 0)
        {
            Debug.LogWarning("RoomSearchDefinition list is assigned, but none of the entries are usable.");
            return false;
        }

        RoomSearchDefinition selectedDefinition = validDefinitions[Random.Range(0, validDefinitions.Count)];
        GameObject roomInstance = Instantiate(selectedDefinition.RoomPrefab, Vector3.zero, Quaternion.identity);
        Debug.Log($"Randomized room from definition: {selectedDefinition.EmpathyLabel}");

        List<Transform> validSpawnPoints = CollectSpawnPoints(roomInstance);
        SpawnDefinitionItems(selectedDefinition, validSpawnPoints);
        SpawnDefinitionThreats(selectedDefinition, validSpawnPoints);
        return true;
    }

    private List<Transform> CollectSpawnPoints(GameObject roomInstance)
    {
        Transform[] spawnPoints = roomInstance.GetComponentsInChildren<Transform>();
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform point in spawnPoints)
        {
            if (point == roomInstance.transform)
            {
                continue;
            }

            string lowerName = point.name.ToLowerInvariant();
            if (lowerName.Contains("spawn") || lowerName.Contains("gameobject"))
            {
                validSpawnPoints.Add(point);
            }
        }

        return validSpawnPoints;
    }

    private void SpawnLegacyItems(RoomData room, List<Transform> spawnPoints)
    {
        if (room.items == null || room.items.Length == 0 || spawnPoints.Count == 0)
        {
            Debug.LogWarning($"No legacy items or spawn points available for room {room.empati}.");
            return;
        }

        List<Transform> availablePoints = new List<Transform>(spawnPoints);
        foreach (SpawnableItem item in room.items)
        {
            if (item == null || item.prefab == null || availablePoints.Count == 0)
            {
                continue;
            }

            if (Random.value <= item.spawnChance)
            {
                int pointIndex = Random.Range(0, availablePoints.Count);
                Transform spawnPoint = availablePoints[pointIndex];

                Instantiate(item.prefab, spawnPoint.position, Quaternion.identity);
                availablePoints.RemoveAt(pointIndex);
            }
        }
    }

    private void SpawnDefinitionItems(RoomSearchDefinition definition, List<Transform> spawnPoints)
    {
        if (definition.ItemSpawns == null || definition.ItemSpawns.Length == 0 || spawnPoints.Count == 0)
        {
            return;
        }

        List<Transform> availablePoints = new List<Transform>(spawnPoints);
        foreach (ItemSpawnEntry itemSpawn in definition.ItemSpawns)
        {
            if (itemSpawn == null || itemSpawn.PickupPrefab == null || availablePoints.Count == 0)
            {
                continue;
            }

            int spawnCount = Mathf.Min(itemSpawn.MaxCount, availablePoints.Count);
            for (int i = 0; i < spawnCount; i++)
            {
                if (Random.value > itemSpawn.SpawnChance || availablePoints.Count == 0)
                {
                    continue;
                }

                int pointIndex = Random.Range(0, availablePoints.Count);
                Transform spawnPoint = availablePoints[pointIndex];
                Instantiate(itemSpawn.PickupPrefab, spawnPoint.position, Quaternion.identity);
                availablePoints.RemoveAt(pointIndex);
            }
        }
    }

    private void SpawnDefinitionThreats(RoomSearchDefinition definition, List<Transform> spawnPoints)
    {
        if (definition.ThreatSpawns == null || definition.ThreatSpawns.Length == 0 || spawnPoints.Count == 0)
        {
            return;
        }

        List<Transform> availablePoints = new List<Transform>(spawnPoints);
        foreach (EnemySpawnEntry threatSpawn in definition.ThreatSpawns)
        {
            if (threatSpawn == null || threatSpawn.EnemyPrefab == null || availablePoints.Count == 0)
            {
                continue;
            }

            int spawnCount = Mathf.Min(threatSpawn.MaxCount, availablePoints.Count);
            for (int i = 0; i < spawnCount; i++)
            {
                if (Random.value > threatSpawn.SpawnChance || availablePoints.Count == 0)
                {
                    continue;
                }

                int pointIndex = Random.Range(0, availablePoints.Count);
                Transform spawnPoint = availablePoints[pointIndex];
                Instantiate(threatSpawn.EnemyPrefab, spawnPoint.position, Quaternion.identity);
                availablePoints.RemoveAt(pointIndex);
            }
        }
    }
}
