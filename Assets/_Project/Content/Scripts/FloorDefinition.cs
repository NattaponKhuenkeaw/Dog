using UnityEngine;

[CreateAssetMenu(menuName = "Dog/Content/Floor Definition", fileName = "FloorDefinition")]
public class FloorDefinition : ScriptableObject
{
    [SerializeField] private int floorNumber = 1;
    [SerializeField] private bool isSafeRoomFloor;
    [SerializeField] private RoomSearchDefinition[] roomSearches;
    [SerializeField] private EncounterDefinition[] hallwayEncounters;

    public int FloorNumber => floorNumber;
    public bool IsSafeRoomFloor => isSafeRoomFloor;
    public RoomSearchDefinition[] RoomSearches => roomSearches;
    public EncounterDefinition[] HallwayEncounters => hallwayEncounters;
}
