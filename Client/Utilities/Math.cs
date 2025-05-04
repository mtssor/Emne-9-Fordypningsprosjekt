using Godot;

namespace NewGameProject.Utilities;

/// <summary>
/// A helper class for Math Functions possibly slightly altered to fit my needs.
/// </summary>
public abstract class Math {
    
    /// <summary>
    /// This is a higher order Lerp that wraps Godot's <see cref="Mathf.Lerp(float,float,float)"/>
    /// </summary>
    /// <param name="from">The start value for interpolation</param>
    /// <param name="to">The destination value for interpolation.</param>
    /// <param name="weight">A value on the range of 0.0 to 1.0, representing the amount of interpolation</param>
    /// <returns>The resulting value of the interpolation</returns>
    public static Vector2 Lerp(Vector2 from, Vector2 to, float weight) {
        float retX = Mathf.Lerp(from.X, to.X, weight);
        float retY = Mathf.Lerp(from.Y, to.Y, weight);
        return new Vector2(retX, retY);
    }
}