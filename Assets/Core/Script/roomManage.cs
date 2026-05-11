using UnityEngine;
using System.Collections.Generic;

public class roomManag : MonoBehaviour
{
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

    void Start()
    {
        // สุ่มห้อง
        int randIndex = Random.Range(0, rooms.Length);
        RoomData selectedRoom = rooms[randIndex];

        // สร้างห้อง
        GameObject roomInstance = Instantiate(selectedRoom.roomPrefab, Vector3.zero, Quaternion.identity);
        Debug.Log("สุ่มได้ห้อง Empati: " + selectedRoom.empati);

        // หา spawn points ทั้งหมดจาก prefab ของห้อง
        Transform[] spawnPoints = roomInstance.GetComponentsInChildren<Transform>();
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform t in spawnPoints)
        {
            // ข้ามตัวแม่ (room เอง)
            if (t == roomInstance.transform) continue;

            // กรองเฉพาะ object ที่ใช้เป็น spawn ได้ (เช่นชื่อขึ้นต้นด้วย "Spawn" หรือ "GameObject")
            if (t.name.ToLower().Contains("spawn") || t.name.ToLower().Contains("gameobject"))
            {
                validSpawnPoints.Add(t);
            }
        }

        // เรียก SpawnItems พร้อมส่งจุด spawn
        SpawnItems(selectedRoom, validSpawnPoints);
    }

    void SpawnItems(RoomData room, List<Transform> spawnPoints)
    {
        if (room.items.Length == 0 || spawnPoints.Count == 0)
        {
            Debug.LogWarning("ไม่มี item หรือ spawn point ในห้อง " + room.empati);
            return;
        }

        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        foreach (var item in room.items)
        {
            if (availablePoints.Count == 0)
                break;

            if (Random.value <= item.spawnChance)
            {
                int pointIndex = Random.Range(0, availablePoints.Count);
                Transform spawnPoint = availablePoints[pointIndex];

                Instantiate(item.prefab, spawnPoint.position, Quaternion.identity);
                Debug.Log($"Spawn {item.prefab.name} ที่ {spawnPoint.position}");

                availablePoints.RemoveAt(pointIndex);
            }
        }
    }
}
