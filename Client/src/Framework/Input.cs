namespace src.Framework;

public static class Input
{
    private static readonly HashSet<Keys> PressedKeys = [];
    private static readonly HashSet<MouseButtons> PressedMouseButtons = [];
    public static Vector2 MousePosition { get; private set; } = Vector2.Zero;

    internal static void Initialize(Form window)
    {
        window.KeyDown += (sender, eventArgs) => PressedKeys.Add(eventArgs.KeyCode);
        window.KeyUp += (sender, eventArgs) => PressedKeys.Remove(eventArgs.KeyCode);
        
        window.MouseDown += (sender, eventArgs) => PressedMouseButtons.Add(eventArgs.Button);
        window.MouseUp += (sender, eventArgs) => PressedMouseButtons.Remove(eventArgs.Button);
        
        window.MouseMove += (sender, eventArgs) => MousePosition = new Vector2(eventArgs.X, eventArgs.Y);
    }
    
    public static bool IsKeyDown(Keys key) => PressedKeys.Contains(key);
    public static bool IsMouseButtonDown(MouseButtons button) => PressedMouseButtons.Contains(button);
}