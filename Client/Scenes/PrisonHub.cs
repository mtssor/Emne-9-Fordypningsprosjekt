using Godot;
using NewGameProject.Entities.Player;

namespace NewGameProject.Scenes;

public partial class PrisonHub : Node2D
{
    private Player Player { get; set; }
    
    public override void _Ready()
    {
        Player = Owner.GetNode<Player>("Player");
        
        Vector2 startPosition = GetNode<Node2D>("PlayerSpawnPosition").Position;
        Player.Position = startPosition;
    }
}