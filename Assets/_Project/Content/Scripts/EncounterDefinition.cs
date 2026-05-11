using UnityEngine;

[CreateAssetMenu(menuName = "Dog/Content/Encounter Definition", fileName = "EncounterDefinition")]
public class EncounterDefinition : ScriptableObject
{
    [SerializeField] private string encounterId = "encounter-id";
    [SerializeField] private string displayName = "New Encounter";
    [SerializeField] private EnemySpawnEntry[] threatSpawns;
    [SerializeField] private ItemSpawnEntry[] rewardSpawns;
    [SerializeField] private float triggerWeight = 1f;

    public string EncounterId => encounterId;
    public string DisplayName => string.IsNullOrWhiteSpace(displayName) ? name : displayName;
    public EnemySpawnEntry[] ThreatSpawns => threatSpawns;
    public ItemSpawnEntry[] RewardSpawns => rewardSpawns;
    public float TriggerWeight => triggerWeight;
}
