using Godot;

namespace NewGameProject.Rooms.Decoration_etc;

public partial class Door : StaticBody2D
{
    private AnimationPlayer _animationPlayer;

    public override void _Ready() => _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    public void Open() {
        _animationPlayer.Play("Open");
    }
}