using Godot;

namespace NewGameProject.Utilities.Items;

[GlobalClass]
public partial class Item : Resource
{
    [Export] public string ItemName = "";
    [Export] public Texture2D ItemIcon = new AtlasTexture();

    public override string ToString() => ItemName;
}