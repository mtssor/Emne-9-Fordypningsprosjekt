using Godot;

namespace NewGameProject.Utilities.Items;

[GlobalClass]
public partial class ItemStack(Item item, int amount = 0) : Node
{
    [Signal] public delegate void ItemChangedEventHandler(Item item);

    public int MaxItemCount => 100;

    private Item _item = item;
    public Item Item
    {
        get => _item;
        set
        {
            _item = value;
            EmitSignal(SignalName.ItemChanged, _item);
        }
    }

    public int Amount { get; set; } = amount;

    public ItemStack() : this(ItemRegistry.Empty) { }


    public bool IsEmpty() => Item == ItemRegistry.Empty;
    public override string ToString() => $"Item: {_item} - {Amount}";
    
}