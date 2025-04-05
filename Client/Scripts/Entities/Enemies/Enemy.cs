using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	private AnimatedSprite2D _animatedSprite2D;
	private AnimatedSprite2D _animatedEffects;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animatedEffects = GetNode<AnimatedSprite2D>("AnimatedEffects");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void TakeDamage(int damageAmount)
	{
		_animatedEffects.Play("Hit");
		GD.Print("Damage:", damageAmount);
	}
}
