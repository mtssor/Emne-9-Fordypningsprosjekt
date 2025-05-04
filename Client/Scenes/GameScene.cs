using Godot;

namespace NewGameProject.Scenes;

public partial class GameScene : Node2D
{
    public GameScene()
    {
        
    }
    
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_focus_next"))
            GetTree().Paused = true;
        if (@event.IsActionPressed("ui_focus_next") && GetTree().Paused)
            GetTree().Paused = false;
    }
}