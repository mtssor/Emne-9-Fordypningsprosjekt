using Godot;
using NewGameProject.Entities.Player;

namespace NewGameProject.Rooms.Traps;

public partial class Spikes : Area2D
{
	[Export] public float SpikeDamage = 10;

	private bool _shouldPlay = true;
	private AnimationPlayer _animationPlayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		
		if (_shouldPlay)
			_animationPlayer.Play("Spikes");
		
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			player.HealthComponent.Damage(SpikeDamage);
		}
	}
}