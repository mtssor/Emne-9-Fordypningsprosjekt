using Godot;

namespace NewGameProject.Scripts;

public partial class Game : Node2D
{
    
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_focus_next"))
            GetTree().Paused = true;
    }
}