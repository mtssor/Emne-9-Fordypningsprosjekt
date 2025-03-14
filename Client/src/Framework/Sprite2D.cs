namespace src.Framework;

public class Sprite2D
{
    public Vector2 Position { get; private set; }
    public Vector2 Scale { get; private set; }
    public readonly Bitmap? Sprite;

    protected Sprite2D(Vector2 position, Vector2 scale, string directory)
    {
        Position = position;
        Scale = scale;
        Sprite = SpritePool.GetSprite(directory);
    }
}