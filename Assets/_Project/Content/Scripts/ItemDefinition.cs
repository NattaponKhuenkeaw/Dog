using UnityEngine;

public enum ItemCategory
{
    Heal,
    Stamina,
    Flashlight,
    Weapon,
    Ammo,
    Currency,
    Charm,
}

[CreateAssetMenu(menuName = "Dog/Content/Item Definition", fileName = "ItemDefinition")]
public class ItemDefinition : ScriptableObject
{
    [SerializeField] private string itemId = "item-id";
    [SerializeField] private string displayName = "New Item";
    [SerializeField] private Sprite icon;
    [SerializeField] private ItemCategory category = ItemCategory.Heal;
    [SerializeField] private int effectValue = 1;
    [SerializeField] private float duration;
    [SerializeField] private float cooldown;
    [SerializeField] private bool isPassive;
    [SerializeField] private bool isConsumable = true;
    [SerializeField] private AudioClip useSound;

    public string ItemId => itemId;
    public string DisplayName => string.IsNullOrWhiteSpace(displayName) ? name : displayName;
    public Sprite Icon => icon;
    public ItemCategory Category => category;
    public int EffectValue => effectValue;
    public float Duration => duration;
    public float Cooldown => cooldown;
    public bool IsPassive => isPassive;
    public bool IsConsumable => isConsumable;
    public AudioClip UseSound => useSound;
}
