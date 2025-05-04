using Godot;
using NewGameProject.Utilities.Items;

namespace NewGameProject.Entities.Player;

[GlobalClass]
public partial class Inventory : Node
{
    [Signal]
    public delegate void InventoryUpdatedEventHandler();

    private ItemStack[] _items = [];
    public const int Size = 10;

    public Inventory()
    {
        for (int i = 0; i < Size; i++)
            _items[i] = new ItemStack(ItemRegistry.Empty);
    }

    public ItemStack AddItem(ItemStack newStack)
    {
        foreach (ItemStack stack in _items)
        {
            if (newStack.IsEmpty())
                break;

            if (stack.Item == newStack.Item && stack.Amount < stack.MaxItemCount)
            {
                stack.Amount += newStack.Amount;
                
                int overload = stack.Amount - stack.MaxItemCount;
                if (overload <= 0)
                    newStack = new ItemStack();
                else newStack.Amount = overload;
            }
        }

        foreach (ItemStack stack in _items)
        {
            if (newStack.IsEmpty())
                break;
            if (!stack.IsEmpty()) 
                continue;
            
            stack.Item = newStack.Item;
            stack.Amount = newStack.Amount;
            newStack = new ItemStack();
        }
        
        EmitSignalInventoryUpdated();
        return newStack;
    }
    
    
}