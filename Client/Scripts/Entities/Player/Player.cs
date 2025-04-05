using Godot;
using NewGameProject.Scripts.Systems.StateMachine;

namespace NewGameProject.Scripts.Entities.Player;

[GlobalClass]
public partial class Player : CharacterBody2D
{
	public AnimatedSprite2D PlayerAnimations { get; set; }
	public StateMachine PlayerStateMachine { get; set; }
	public Components.PlayerMoveComponent PlayerMoveComponent  { get; set; }

	private AnimationPlayer _swordAnimation;
	
	public Node2D Sword { get; set; }
	
	public override void _Ready()
	{
		PlayerAnimations = GetNode<AnimatedSprite2D>("Animations");
		PlayerStateMachine = GetNode<StateMachine>("StateMachine");
		PlayerMoveComponent = GetNode<Components.PlayerMoveComponent>("MoveComponent");

		Sword = GetNode<Node2D>("Sword");
		_swordAnimation = Sword.GetNode<AnimationPlayer>("SwordAnimation");

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
		Sword.Scale = Sword.Scale.Y switch
		{
			1 when mouseDirection.X < 0 => Sword.Scale with { Y = -1 },
			-1 when mouseDirection.X > 0 => Sword.Scale with { Y = 1 },
			_ => Sword.Scale
		};
		
		if (Input.IsActionPressed("ui_attack") && !_swordAnimation.IsPlaying())
			_swordAnimation.Play("sword_attack");

		PlayerStateMachine.ProcessFrame(delta);
	}
}