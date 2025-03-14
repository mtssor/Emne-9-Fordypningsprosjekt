namespace src.Framework;

public class Shape2D
{
    public Vector2 Position { get; private set; }
    public Vector2 Scale { get; private set; }
    private string Tag { get; }
    
    private bool _isDestroyed;

    protected Shape2D(Vector2 position, Vector2 scale, string tag)
    {
        Position = position;
        Scale = scale;
        Tag = tag;
        
        Log.Info($"[SHAPE2D]({Tag}) - has been registered");
        // Engine.RegisterShape(this);
    }

    public void SetPosition(Vector2 newPosition)
    {
        if (!_isDestroyed) 
            Position = newPosition;
    }

    public void SetScale(Vector2 newScale)
    {
        if (!_isDestroyed)
            Scale = newScale;
    }

    // public void DestroySelf()
    // {
    //     if (_isDestroyed)
    //         return;
    //     
    //     Log.Info($"[SHAPE2D]({Tag}) - has been unregistered");
    //     Engine.UnregisterShape(this);
    //     _isDestroyed = true;
    // }
}