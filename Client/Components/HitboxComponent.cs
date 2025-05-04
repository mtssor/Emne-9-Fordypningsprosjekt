using Godot;

namespace NewGameProject.Components;

/// <summary>
/// Defines a hitbox. Used by weapons or enemies to signal when overlapping with hurtboxes
/// </summary>
[GlobalClass]
public partial class HitboxComponent : Area2D
{
	[Signal]
	public delegate void AreaEnteredEventHandler(Area2D area);
	
	public HitboxComponent()
	{
		// Default collision setup. Layer 5 is for Player attack, Mask 3 is for enemy hurtbox
		CollisionLayer = 5;
		CollisionMask = 3;
	}

	public override void _Ready()
	{
		Connect("area_entered", Callable.From<Area2D>(area => EmitSignal("AreaEntered", area)));
	}

	
}
