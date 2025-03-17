using RLNET;
using RogueSharp_Tutorial.Core;
using RogueSharp_Tutorial.Systems;
using RogueSharp.Random;

namespace RogueSharp_Tutorial
{
    public class Game
    {
        // width and height of "screen"
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;
        
        // Map console, takes up most of screen. Is where map will be drawn
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;
        
        // Below map console we have a message console which displays attacks and other info
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;
        
        // Stat console to the right of map. Displays player and monster stats
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static RLConsole _statConsole;
        
        // Above map is the inventory console, shows player equipment, abilities, items, etc
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;
        
        private static bool _renderRequired = true;
        
        // temp member variable to test that MessageLog is working 
        //private static int _steps = 0;
        
        public static Player Player { get; set; }
        public static DungeonMap DungeonMap { get; private set; }
        public static MessageLog MessageLog { get; private set; }
        public static SchedulingSystem SchedulingSystem { get; private set; }
        public static CommandSystem CommandSystem { get; private set; }

        // singleton of IRandom used throughout the game when generating random numbers
        public static IRandom Random { get; private set; }
        
        public static void Main()
        {
            // establish the seed for the random number generator from the currecnt time 
            int seed = (int) DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);
            
            // create new MessageLog and print the random seed used to generate lvl
            MessageLog = new MessageLog();
            MessageLog.Add("The rogue arrives on level 1");
            MessageLog.Add($"Level created with seed '{seed}'");
            
            
            // exact name of bitmap font file
            string fontFileName = "terminal8x8.png";
            // "Title" on top of console window. 
            // Now also including seed 
            string consoleTitle = $"RogueSharp Tutorial - Level 1 - Seed: {seed}";
            // Tell RLNet to use the bitmap font, and that each tile is 8x8 pixels
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);
            
            // Init the sub console that will Blit to the root console
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);
            
            SchedulingSystem = new SchedulingSystem();
            
            // creates player
            //Player = new Player();
            // creates dungeon map
            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 13, 7);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();
            
            CommandSystem = new CommandSystem();
            
            // handler for RLNet update event
            _rootConsole.Update += OnRootConsoleUpdate;
            //handler for RLNet Render event 
            _rootConsole.Render += OnRootConsoleRender;
            
            //_rootConsole.Print(10, 10, "It worked!", RLColor.White);
            
            // Set background color and text for each console
            // setting different ones to verify they are in correct positions
            //_mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Colors.FloorBackground);
            //_mapConsole.Print(1, 1, "Map", Colors.TextHeading);
            
            //_messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Swatch.DbDeepWater);
            //_messageConsole.Print(1, 1, "Messages", Colors.TextHeading);
            
            //_statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.DbOldStone);
            //_statConsole.Print(1, 1, "Stats", Colors.TextHeading);
            
            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
            _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);
            
            // Begin RLNet game loop
            _rootConsole.Run();
        }
        
        // Event handler RLNet update event
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            // capturing key presses
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();

            if (CommandSystem.IsPlayerTurn)
            {
                if (keyPress != null)
                {
                    if (keyPress.Key == RLKey.Up)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                    }
                    else if (keyPress.Key == RLKey.Down)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                    }
                    else if (keyPress.Key == RLKey.Left)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                    }
                    else if (keyPress.Key == RLKey.Right)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                    }
                    else if (keyPress.Key == RLKey.Escape)
                    {
                        _rootConsole.Close();
                    }
                }
                if (didPlayerAct)
                {
                    // testing that MessageLog works
                    //MessageLog.Add($"Step #{++_steps}");
                
                    _renderRequired = true;
                    CommandSystem.EndPlayerTurn();
                }
            }
            else
            {
                CommandSystem.ActivateMonsters();
                _renderRequired = true;
            }
        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            // dont bother redrawing all of the consoles if nothing has changed
            if (_renderRequired)
            {
                _mapConsole.Clear();
                _statConsole.Clear();
                _messageConsole.Clear();
                
                // draws dungeon map
                DungeonMap.Draw(_mapConsole, _statConsole);
                // draws player
                Player.Draw(_mapConsole, DungeonMap);
                // draw player stats
                Player.DrawStats(_statConsole);
                // draw message log
                MessageLog.Draw(_messageConsole);
                
                // Blit the sub consoles to the root ocnsole in the correct locations
                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);
                
                
            
                // Tell RLNet to draw the console
                _rootConsole.Draw();
                
                _renderRequired = false;
            }
        }
        
        // temp member variable to test that MessageLog is working 
        
    }
}
