using Godot;

namespace NewGameProject;

public partial class EnemySpawnEffect : AnimatedSprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IsPlaying();
		AnimationFinished += OnAnimationFinished;
	}

	public void OnAnimationFinished()
	{
		QueueFree();
	}
}