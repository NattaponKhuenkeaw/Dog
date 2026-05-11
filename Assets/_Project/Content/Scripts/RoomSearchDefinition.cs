using UnityEngine;

[CreateAssetMenu(menuName = "Dog/Content/Room Search Definition", fileName = "RoomSearchDefinition")]
public class RoomSearchDefinition : ScriptableObject
{
    [SerializeField] private string roomId = "room-id";
    [SerializeField] private string empathyLabel = "Room";
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private ItemSpawnEntry[] itemSpawns;
    [SerializeField] private EnemySpawnEntry[] threatSpawns;
    [SerializeField] private float searchProgressRequired = 100f;

    public string RoomId => roomId;
    public string EmpathyLabel => string.IsNullOrWhiteSpace(empathyLabel) ? name : empathyLabel;
    public GameObject RoomPrefab => roomPrefab;
    public ItemSpawnEntry[] ItemSpawns => itemSpawns;
    public EnemySpawnEntry[] ThreatSpawns => threatSpawns;
    public float SearchProgressRequired => searchProgressRequired;
}
