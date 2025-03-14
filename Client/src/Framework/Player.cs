namespace src.Framework;

public class Player
{
    public string Tag { get; }
    private int MoveSpeed { get; }
    
    public readonly Bitmap? Sprite;
    public Vector2 Position { get; }
    public Vector2 Scale { get; }
    public Hurtbox? Hurtbox { get; set; }
    public int Health => Hurtbox!.Health;

    public Player(string tag, int health, int moveSpeed, string directory, Vector2 position, Vector2 scale)
    {
        Tag = tag;
        MoveSpeed = moveSpeed;
        
        Position = position;
        Scale = scale;
        
        Sprite = SpritePool.GetSprite(directory);
        Hurtbox = new Hurtbox(new Rectangle(
            (int)position.X, (int)position.Y, 
            (int)scale.X, (int)scale.Y), health);
        
        Log.Info($"[PLAYER]({Tag}) - Has Been Registered");
        Engine.RegisterPlayer(this);
    }

    public void Movement()
    {
        if (Input.IsKeyDown(Keys.D))
            Position.X += MoveSpeed;
        if (Input.IsKeyDown(Keys.A))
            Position.X -= MoveSpeed;
        
        if (Input.IsKeyDown(Keys.W))
            Position.Y -= MoveSpeed;
        if (Input.IsKeyDown(Keys.S))
            Position.Y += MoveSpeed;
    }
    
    public void DestroySelf()
    {
        Log.Info("Game Over!"); //TODO: Remove when a start function is created
        Hurtbox = null;
        Engine.UnregisterPlayer();
    }
}