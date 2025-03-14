namespace src.Framework;

internal sealed class Canvas : Form
{
    public Canvas()
    {
        DoubleBuffered = true;
    }
}

public abstract class Engine
{
    private readonly Canvas _window;
    private readonly bool _isRunning;
    
    // private static readonly List<Shape2D> AllShapes = [];
    // private static readonly List<Sprite2D> AllSprites = [];

    private static Player? _player;
    private static readonly Dictionary<string, Enemy> Enemies = [];
    
    protected Color BackgroundColor;
    
    private const int TargetFps = 30;
    private const double FrameTime = 1000.0 / TargetFps;
    
    protected Engine(Vector2 screenSize, string title)
    {
        Log.Info("Game is starting...");

        _window = new Canvas
        {
            Size = new Size((int)screenSize.X, (int)screenSize.Y),
            Text = title
        };
        
        Input.Initialize(_window);
        
        _window.Paint += Renderer;
        
        Thread gameThread = new(GameLoop);
        _isRunning = true;
        gameThread.Start();
        
        Application.Run(_window);
    }
    
    private void GameLoop()
    {
        OnLoad();

        double previousTime = Environment.TickCount64;
        double lag = 0.0;
        
        while (_isRunning)
        {
            double currentTime = Environment.TickCount64;
            double deltaTime = currentTime - previousTime;
            
            previousTime = currentTime;
            lag += deltaTime;
            
            // Updates only fixed times
            while (lag >= FrameTime)
            {
                OnUpdate();
                lag -= FrameTime;
            }

            // Render as often as possible
            _window.BeginInvoke((MethodInvoker)delegate { _window.Refresh(); });
            
            // To prevent high CPU usage
            Thread.Sleep(1);
        }
    }
    
    private void Renderer(object? sender, PaintEventArgs eventArgs)
    {
        Graphics graphics = eventArgs.Graphics;
        graphics.Clear(BackgroundColor);

        if(_player != null)
            graphics.DrawImage(_player.Sprite, _player.Position.X, _player.Position.Y, _player.Scale.X, _player.Scale.Y);
        foreach (Enemy enemy in Enemies.Values)
            graphics.DrawImage(enemy.Sprite, enemy.Position.X, enemy.Position.Y, enemy.Scale.X, enemy.Scale.Y);
    }

    // public static void RegisterShape(Shape2D shape) => AllShapes.Add(shape);
    // public static void UnregisterShape(Shape2D shape) => AllShapes.Remove(shape);
    // public static void RegisterSprite(Sprite2D sprite) => AllSprites.Add(sprite);
    // public static void UnregisterSprite(Sprite2D sprite) => AllSprites.Remove(sprite);
    
    public static void RegisterPlayer(Player player) => _player = player;
    public static void UnregisterPlayer() => _player = null;

    public static void RegisterEnemy(Enemy enemy) => Enemies.Add(enemy.Tag, enemy);
    public static void UnregisterEnemy(Enemy enemy) => Enemies.Remove(enemy.Tag);
    
    protected abstract void OnLoad();
    protected abstract void OnUpdate();
}