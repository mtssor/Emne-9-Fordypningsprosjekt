using Godot;
using NewGameProject.Scripts.Components.Interfaces;

namespace NewGameProject.Scripts.Systems.StateMachine;

[GlobalClass]
public partial class State : Node
{
    [Export] public string AnimationName;
    public AnimatedSprite2D Animations;
    public CharacterBody2D Parent;
    public IMoveComponent MoveComponent;

    public virtual void Enter()
    {
        Animations.Play(AnimationName);
    }
    public void Exit() { }
    
    public virtual State ProcessInput(InputEvent @event) => null;
    public virtual State ProcessFrame(double deltaTime) => null;
    public virtual State ProcessPhysics(double deltaTime) => null;
}