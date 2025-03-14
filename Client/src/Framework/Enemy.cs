namespace src.Framework;

public class Enemy
{
    public string Tag { get; }
    public int Damage { get; }
    public Hitbox Hitbox { get; private set; }
    public readonly Bitmap? Sprite;
    public Vector2 Position { get; }
    public Vector2 Scale { get; }

    public Enemy(string tag, int damage, string directory, Vector2 position, Vector2 scale)
    {
        Tag = tag;
        Damage = damage;
        
        Position = position;
        Scale = scale;
        
        Sprite = SpritePool.GetSprite(directory);
        Hitbox = new Hitbox(new Rectangle(
            (int)position.X, (int)position.Y, 
            (int)scale.X, (int)scale.Y), damage);
        
        Log.Info($"[ENEMY]({Tag}) - Has Been Registered");
        Engine.RegisterEnemy(this);
    }

    public void DestroySelf() => Engine.UnregisterEnemy(this);
}