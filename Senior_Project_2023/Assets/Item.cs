using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id;
    public string name;
    public int ammo;
    public Sprite icon;
    public Item(int id, string name, int ammo) {
        this.name = name;
        this.id = id;
        this.ammo = ammo;
        this.icon = Resources.Load<Sprite>("Artwork/" + name);
    }
    public Item(Item item) {
        this.id = item.id;
        this.name = item.name;
        this.ammo = item.ammo;
        this.icon = Resources.Load<Sprite>("Artwork/" + name);
    }
}
