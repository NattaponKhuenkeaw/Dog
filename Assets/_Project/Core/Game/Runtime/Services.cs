public static class Services
{
    public static HealthSystem Health { get; internal set; }
    public static EnergySystem Energy { get; internal set; }
    public static FlashlightSystem Flashlight { get; internal set; }
    public static InventoryManager Inventory { get; internal set; }
    public static DoorLockRegistry Doors { get; internal set; }
    public static SessionManager Session { get; internal set; }
    public static SceneLoader SceneLoader { get; internal set; }
}
