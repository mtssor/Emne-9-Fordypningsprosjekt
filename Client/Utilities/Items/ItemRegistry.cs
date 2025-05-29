using System.Collections.Generic;
using Godot;

namespace NewGameProject.Utilities.Items;

public partial class ItemRegistry : Node
{
    public static Item Empty = new();
    public Item[] AllItems = [Empty];
    public Dictionary<string, Item> ItemDictionary = new();

    public override void _Ready()
    {
        RegisterItems();
        Empty.ItemName = "Empty";
    }

    public void RegisterItems()
    {
        foreach (Item item in AllItems)
            ItemDictionary[item.ItemName] = item;
    }
    
    public Item GetItemByName(string name) => ItemDictionary[name];
}