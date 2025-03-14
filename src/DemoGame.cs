using src.Framework;

namespace src;

// TODO: Add a meat chunk item that heals you when you attack enemies when getting around to adding items
public class DemoGame() : Engine(new Vector2(615, 515), "Engine Demo")
{
    private Player? _player;
    private Enemy? _enemy;

    protected override void OnLoad()
    {
        BackgroundColor = Color.Black;

        _player = new Player(
            tag: "Player",
            health: 100,
            moveSpeed: 10,
            directory: "Character_animation/priests_idle/priest1/v1/priest1_v1_1",
            position: new Vector2(10, 10),
            scale: new Vector2(32, 32));
        
        _enemy = new Enemy(
            tag: "skeleton1",
            damage: 10,
            directory: "Character_animation/monsters_idle/skeleton1/v1/skeleton_v1_1",
            position: new Vector2(50, 50),
            scale: new Vector2(32, 32));
    }

    
    protected override void OnUpdate()
    {
        _player?.Movement();
        _player?.Hurtbox?.UpdateBounds(new Point((int)_player.Position.X, (int)_player.Position.Y));

        if (_player?.Hurtbox != null && _enemy.Hitbox.Bounds.IntersectsWith(_player.Hurtbox.Bounds))
        {
            _player.Hurtbox.ApplyDamage(_enemy.Damage);
            Log.Info($"Collision! Player health is now: {_player.Hurtbox.Health}");
        }

        if (_player?.Hurtbox is { Health: 0 })
        {
            Log.Info("Game Over!");
            _player.DestroySelf();
        }
    }
}