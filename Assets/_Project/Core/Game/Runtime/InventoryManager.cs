using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public event Action OnInventoryChanged;

    public List<ItemData> Items { get; } = new List<ItemData>();
    public int MaxInventorySize { get; private set; } = 3;
    private Dictionary<ItemCategory, Action<ItemData>> useHandlers;

    private void Awake()
    {
        Services.Inventory = this;
        useHandlers = new Dictionary<ItemCategory, Action<ItemData>>
        {
            { ItemCategory.Heal, item => Services.Health?.Heal(item.Value) },
            { ItemCategory.Stamina, item => Services.Energy?.AddEnergy(item.Value) },
            { ItemCategory.Flashlight, item => Services.Flashlight?.Recharge(item.Value) },
            { ItemCategory.Weapon, item => Debug.Log($"Weapon item '{item.ItemName}' is not wired yet.") },
            { ItemCategory.Ammo, item => Debug.Log($"Ammo item '{item.ItemName}' is not wired yet.") },
            { ItemCategory.Currency, item => Debug.Log($"Currency item '{item.ItemName}' is not wired yet.") },
            { ItemCategory.Charm, item => Debug.Log($"Charm item '{item.ItemName}' is passive and not manually consumed.") },
        };
    }

    public void Configure(int maxInventorySize)
    {
        MaxInventorySize = Mathf.Max(1, maxInventorySize);
        NotifyInventoryChanged();
    }

    public bool CanAddItem => Items.Count < MaxInventorySize;

    public bool AddItem(ItemData item)
    {
        if (item == null || !CanAddItem)
        {
            return false;
        }

        Items.Add(item);
        NotifyInventoryChanged();
        return true;
    }

    public void UseItemAt(int index)
    {
        if (index < 0 || index >= Items.Count)
        {
            return;
        }

        UseItem(Items[index]);
    }

    public void UseItem(ItemData item)
    {
        if (item == null)
        {
            return;
        }

        if (item.HasDefinition)
        {
            if (useHandlers.TryGetValue(item.Category, out Action<ItemData> handler))
            {
                handler?.Invoke(item);
            }
        }
        else
        {
            switch (item.Type)
            {
                case ItemPickup.ItemType.Heal:
                    Services.Health?.Heal(item.Value);
                    break;
                case ItemPickup.ItemType.Energy:
                    Services.Energy?.AddEnergy(item.Value);
                    break;
                case ItemPickup.ItemType.Baterry:
                    Services.Flashlight?.Recharge(item.Value);
                    break;
            }
        }

        if (item.IsConsumable)
        {
            Items.Remove(item);
        }

        NotifyInventoryChanged();
    }

    public void ResetState()
    {
        Items.Clear();
        NotifyInventoryChanged();
    }

    public void NotifyInventoryChanged()
    {
        OnInventoryChanged?.Invoke();
    }
}
