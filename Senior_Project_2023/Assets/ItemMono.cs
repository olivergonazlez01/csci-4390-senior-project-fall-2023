using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMono : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Item item;

    private void Awake() {
        CreateItems();
    }

    public Item GetItem(int id) {
        return items.Find(item => item.id == id);
    }

    void CreateItems()
    {
        items = new List<Item>() {
            new Item(0, "Pistol", 100)
        };
    }
}
