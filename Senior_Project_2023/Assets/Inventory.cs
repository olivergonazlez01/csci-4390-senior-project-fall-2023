using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> inv = new List<Item>();
    public ItemMono items;

    private void Start() {
        GiveItem(0);
        RemoveItem(0);
    }

    public void GiveItem(int id)
    {
        Item newitem = items.GetItem(id);
        inv.Add(newitem);
        Debug.Log("added item " + newitem.name);
    }

    public Item FindItem(int id) {
        return inv.Find(item => item.id == id);
    }
    
    public void RemoveItem(int id) {
        Item item = FindItem(id);
        if (item != null) {
            inv.Remove(item);
            Debug.Log("removed item " + item.name);
        }
    }
}
