using Godot;
using NewGameProject.Scripts.Systems.StateMachine;

namespace NewGameProject.Scripts.Entities.Player;

[GlobalClass]
public partial class Player : CharacterBody2D
{
	public AnimatedSprite2D PlayerAnimations { get; set; }
	public StateMachine PlayerStateMachine { get; set; }
	public PlayerMoveComponent PlayerMoveComponent  { get; set; }
	
	public Node2D Sword { get; set; }
	
	public override void _Ready()
	{
		PlayerAnimations = GetNode("Animations") as AnimatedSprite2D;
		PlayerStateMachine = GetNode("StateMachine") as StateMachine;
		PlayerMoveComponent = GetNode("MoveComponent") as PlayerMoveComponent;
		
		Sword = GetNode("Sword") as Node2D;
		
		PlayerStateMachine.Init(this, PlayerAnimations, PlayerMoveComponent);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		PlayerStateMachine.ProcessInput(@event);
	}

	public override void _PhysicsProcess(double delta)
	{
		PlayerStateMachine.ProcessPhysics(delta);
	}

	public override void _Process(double delta)
	{
		Vector2 mouseDirection = (GetGlobalMousePosition() - GlobalPosition).Normalized();
		PlayerAnimations.FlipH = mouseDirection.X switch
		{
			> 0 when PlayerAnimations.FlipH => false,
			< 0 when !PlayerAnimations.FlipH => true,
			_ => PlayerAnimations.FlipH
		};

		Sword.Rotation = mouseDirection.Angle();
		if (Sword.Scale.Y == 1 && mouseDirection.X < 0)
			Sword.Scale = Sword.Scale with { Y = -1 };
		else if (Sword.Scale.Y == -1 && mouseDirection.X > 0)
			Sword.Scale = Sword.Scale with { Y = 1 };
		

		PlayerStateMachine.ProcessFrame(delta);
	}
}