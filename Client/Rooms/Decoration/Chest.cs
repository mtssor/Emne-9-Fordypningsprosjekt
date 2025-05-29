using Godot;
using NewGameProject.Entities.Player;
using NewGameProject.Globals;

public partial class Chest : Area2D
{
	public float DamageUp = 1.20f;
	private AnimatedSprite2D _animatedSprite;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			if (Input.IsActionPressed("ui_accept"))
			{
				_animatedSprite.Play("Open");
				GD.Print($"Received Damage Up : {DamageUp}");
				SavedData.AllDamageUp.Add(DamageUp);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
