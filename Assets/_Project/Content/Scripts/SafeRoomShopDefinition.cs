using System;
using UnityEngine;

[Serializable]
public class SafeRoomShopEntry
{
    [SerializeField] private ItemDefinition item;
    [SerializeField] private int price = 1;
    [SerializeField] private int stock = 1;

    public ItemDefinition Item => item;
    public int Price => Mathf.Max(0, price);
    public int Stock => Mathf.Max(0, stock);
}

[CreateAssetMenu(menuName = "Dog/Content/Safe Room Shop Definition", fileName = "SafeRoomShopDefinition")]
public class SafeRoomShopDefinition : ScriptableObject
{
    [SerializeField] private string shopId = "safe-room-shop";
    [SerializeField] private SafeRoomShopEntry[] entries;

    public string ShopId => shopId;
    public SafeRoomShopEntry[] Entries => entries;
}
