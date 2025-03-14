namespace src.Framework;

/// <param name="x">Vertical Coordinates</param>
/// <param name="y">Horizontal Coordinates</param>
public class Vector2(float x = 0, float y = 0)
{
    public float X { get; set; } = x;
    public float Y { get; set; } = y;

    /// <summary>
    /// Returns the X & Y coordinates as 0
    /// </summary>
    public static readonly Vector2 Zero = new();
    
    /// <summary>
    /// Adds two vectors.
    /// </summary>
    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
    
    /// <summary>
    /// Subtracts one vector from another.
    /// </summary>
    public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
    
    /// <summary>
    /// Multiplies a vector by a scalar
    /// </summary>
    public static Vector2 operator *(Vector2 vector, float scalar) => new(vector.X * scalar, vector.Y * scalar);
    
    /// <summary>
    /// Calculates the magnitude of the vector
    /// </summary>
    private float Magnitude() => MathF.Sqrt(X * X + Y * Y);

    /// <summary>
    /// Returns a normalized (unit length) version of the vector.
    /// </summary>
    public Vector2 Normalize()
    {
        float magnitude = Magnitude();
        return magnitude > 0 ? new Vector2(X / magnitude, Y / magnitude) : Zero;
    }
    
    public override string ToString() => $"({X}, {Y})";
}