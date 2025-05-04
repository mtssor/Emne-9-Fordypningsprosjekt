using Godot;
using NewGameProject.Scripts.Components.Interfaces;

namespace NewGameProject.Entities.Enemies.Goblin;

[GlobalClass]
public partial class GoblinMoveComponent: Node2D, IMoveComponent 
{
    public Player.Player Player;

    public Vector2 Direction = Vector2.Zero;
    
    public const int MaxDistanceToPlayer = 80;
    public const int MinDistanceToPlayer = 40;

    public float DistanceToPlayer;

    private bool _canAttack;

    public override void _Ready()
    {
        Player = Owner.GetTree().CurrentScene.GetNode<Player.Player>("Player");
    }

    public Vector2 GetMovementDirection() 
    {
        return Direction = DistanceToPlayer > MaxDistanceToPlayer && 
                           DistanceToPlayer < MinDistanceToPlayer ? Direction.Normalized() : Vector2.Zero;
    }

    public float MoveSpeed { get; }

    public Vector2 GetPathToPlayer() 
    {
        return GetOwner<Goblin>().NavigationAgent.TargetPosition = Player.Position;
    }
    
    public void OnPathTimerTimeOut()
    {
        if (!IsInstanceValid(Player))
        {
            GetOwner<Goblin>().PathTimer.Stop();
            Direction = Vector2.Zero;
            return;
        }
        
        DistanceToPlayer = (Player.Position - GlobalPosition).Length();
        switch (DistanceToPlayer) 
        {
            case > MaxDistanceToPlayer:
                GetPathToPlayer();
                break;
            case < MinDistanceToPlayer:
                GetPathToMoveAwayFromPlayer();
                break;
            default: 
            {
                GetOwner<Goblin>().AimRayCast.TargetPosition = Player.Position - GlobalPosition;
                
                // if (_canAttack && GetOwner<Goblin>().StateMachine.CurrentState.IsClass("IdleState") && 
                //     GetOwner<Goblin>().AimRayCast.IsColliding() == false) 
                // {
                //     _canAttack = false;
                //     GetOwner<Goblin>().AttackTimer.Start();
                // }
                break;
            }
        }
    }

    private void GetPathToMoveAwayFromPlayer() 
    {
        Vector2 direction = (GlobalPosition - Player.Position).Normalized();
        GetOwner<Goblin>().NavigationAgent.TargetPosition = GlobalPosition + direction * 100;
    }
    
    public void ChasePlayer() 
    {
        if (GetOwner<Goblin>().NavigationAgent.IsTargetReached())
            return;
		
        Vector2 vectorToNextPoint = GetOwner<Goblin>().NavigationAgent.GetNextPathPosition() - GlobalPosition;
        Direction = vectorToNextPoint;
		
        GetOwner<Goblin>().Animations.FlipH = vectorToNextPoint.X switch 
        {
            > 0 when GetOwner<Goblin>().Animations.FlipH => false,
            < 0 when !GetOwner<Goblin>().Animations.FlipH => true,
            _ => GetOwner<Goblin>().Animations.FlipH
        };
    }
}