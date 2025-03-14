namespace src.Framework;

public static class SpritePool
{
    private static readonly Dictionary<string, Bitmap> CachedSprites = new();

    public static Bitmap GetSprite(string directory)
    {
        if (CachedSprites.TryGetValue(directory, out Bitmap? sprite))
            return sprite;
        
        Image temporaryImage = Image.FromFile($"Assets/Sprites/{directory}.png");
        sprite = new Bitmap(temporaryImage);
        CachedSprites[directory] = sprite;
            
        Log.Info($"[SPRITE2D]({directory}) - Has been loaded and cached");
        return sprite;
    }
}