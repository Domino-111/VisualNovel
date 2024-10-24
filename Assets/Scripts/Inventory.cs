using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item
{
    Sword,
    ChestKey,
    HealthPotion,
}

public class Inventory
{
    private List<Item> currentItems = new List<Item>();

    public void Collect(Item item)
    {
        currentItems.Add(item);
    }

    public bool Has(Item itme)
    {
        return currentItems.Contains(itme);
    }
}
