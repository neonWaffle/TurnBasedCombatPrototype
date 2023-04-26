using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeField] ItemSO[] startingItems;
    [SerializeField] int[] startingItemsQuantity;
    public List<Item> Items { get; private set; } = new List<Item>();

    void Awake()
    {
        for (int i = 0; i < startingItems.Length; i++)
        {
            AddItem(startingItems[i], startingItemsQuantity[i]);
        }
    }

    void AddItem(ItemSO itemSO, int quantity)
    {
        var item = Items.FirstOrDefault(item => item.ItemSO == itemSO);

        if (item != null)
        {
            item.IncreaseQuantity(quantity);
        }
        else
        {
            item = new Item(itemSO, quantity);
            Items.Add(item);
        }

        item.OnUsedUp += RemoveItem;
    }

    void RemoveItem(Item item)
    {
        item.OnUsedUp -= RemoveItem;
        Items.Remove(item);
    }
}
