using Godot;
using NewGameProject.Scripts.Models;

namespace NewGameProject.Scripts.Components;

[GlobalClass]
public partial class HitboxComponent : Area2D
{
	[Signal]
	public delegate void AreaEnteredEventHandler(Area2D area);
	
	public HitboxComponent()
	{
		CollisionLayer = 5;
		CollisionMask = 3;
	}

	public override void _Ready()
	{
		//Monitoring = false;
		//Monitorable = true;
		Connect("area_entered", Callable.From<Area2D>(area => EmitSignal("AreaEntered", area)));
	}

	
}
