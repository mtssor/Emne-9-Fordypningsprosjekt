namespace src.Framework;

public class Hitbox(Rectangle bounds, int damage)
{
    public Rectangle Bounds { get; private set; } = bounds;
    public int Damage { get; set; } = damage;

    public bool Intersects(Rectangle other) => Bounds.IntersectsWith(other);
    public void UpdateBounds(Point newTopLeft) => Bounds = new Rectangle(newTopLeft, Bounds.Size);
}