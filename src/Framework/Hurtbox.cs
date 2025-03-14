namespace src.Framework;

public class Hurtbox(Rectangle bounds, int health)
{
    public Rectangle Bounds { get; private set; } = bounds;
    public int Health { get; set; } = health;


    public bool Intersects(Rectangle other) => Bounds.IntersectsWith(other);
    public void UpdateBounds(Point newTopLeft) => Bounds = new Rectangle(newTopLeft, Bounds.Size);
    public void ApplyDamage(int damage) => Health -= damage;
}