using UnityEngine;

[System.Serializable]
public class ItemData
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    [SerializeField] private ItemPickup.ItemType legacyType;
    [SerializeField] private int value;
    [SerializeField] private ItemDefinition definition;

    public ItemData(string name, Sprite ic, ItemPickup.ItemType itemType, int amount)
    {
        itemName = name;
        icon = ic;
        legacyType = itemType;
        value = amount;
    }

    public ItemData(ItemDefinition itemDefinition)
    {
        definition = itemDefinition;
        itemName = itemDefinition != null ? itemDefinition.DisplayName : string.Empty;
        icon = itemDefinition != null ? itemDefinition.Icon : null;
        value = itemDefinition != null ? itemDefinition.EffectValue : 0;
        legacyType = MapLegacyType(itemDefinition != null ? itemDefinition.Category : ItemCategory.Heal);
    }

    public string ItemName => definition != null ? definition.DisplayName : itemName;
    public Sprite Icon => definition != null ? definition.Icon : icon;
    public int Value => definition != null ? definition.EffectValue : value;
    public ItemDefinition Definition => definition;
    public bool HasDefinition => definition != null;
    public ItemCategory Category => definition != null ? definition.Category : MapCategory(legacyType);
    public bool IsConsumable => definition == null || definition.IsConsumable;
    public ItemPickup.ItemType Type => legacyType;

    private static ItemCategory MapCategory(ItemPickup.ItemType itemType)
    {
        switch (itemType)
        {
            case ItemPickup.ItemType.Heal:
                return ItemCategory.Heal;
            case ItemPickup.ItemType.Energy:
                return ItemCategory.Stamina;
            case ItemPickup.ItemType.Baterry:
                return ItemCategory.Flashlight;
            default:
                return ItemCategory.Heal;
        }
    }

    private static ItemPickup.ItemType MapLegacyType(ItemCategory category)
    {
        switch (category)
        {
            case ItemCategory.Stamina:
                return ItemPickup.ItemType.Energy;
            case ItemCategory.Flashlight:
                return ItemPickup.ItemType.Baterry;
            default:
                return ItemPickup.ItemType.Heal;
        }
    }
}
