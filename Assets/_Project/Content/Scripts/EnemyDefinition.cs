using UnityEngine;

public enum EnemyRole
{
    Stalker,
    RoomThreat,
    HallwayRush,
    HidePunish,
}

public enum CounterplayType
{
    Run,
    Hide,
    Flashlight,
    Revolver,
    EnergyDrink,
    SafeRoom,
}

[CreateAssetMenu(menuName = "Dog/Content/Enemy Definition", fileName = "EnemyDefinition")]
public class EnemyDefinition : ScriptableObject
{
    [SerializeField] private string enemyId = "enemy-id";
    [SerializeField] private string displayName = "New Enemy";
    [SerializeField] private EnemyRole role = EnemyRole.RoomThreat;
    [SerializeField] private float baseSpeed = 1f;
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private float warningDuration = 1f;
    [SerializeField] private float despawnDelay = 1f;
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private CounterplayType[] counterplays;
    [SerializeField] private AudioClip warningAudio;
    [SerializeField] private AudioClip attackAudio;

    public string EnemyId => enemyId;
    public string DisplayName => string.IsNullOrWhiteSpace(displayName) ? name : displayName;
    public EnemyRole Role => role;
    public float BaseSpeed => baseSpeed;
    public int BaseDamage => baseDamage;
    public float WarningDuration => warningDuration;
    public float DespawnDelay => despawnDelay;
    public float DetectionRange => detectionRange;
    public CounterplayType[] Counterplays => counterplays;
    public AudioClip WarningAudio => warningAudio;
    public AudioClip AttackAudio => attackAudio;
}
