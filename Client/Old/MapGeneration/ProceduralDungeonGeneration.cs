#nullable enable
    using System;
    using Godot;
    using Godot.Collections;

    namespace NewGameProject.Old.MapGeneration;

// TODO: Very much unfinished!
public partial class ProceduralDungeonGeneration : Node2D
{
    [Export] private Vector2I _dimensions = new(7, 5);
    [Export] private Vector2I _startPoint = new(-1, 0);
    [Export] private Vector2I _endPoint = new(1, 0);
    [Export] private int _criticalPathLength = 13;

    private readonly Random _random = new();
    private Array<Array<Variant>> _dungeon = [];

    public override void _Ready()
    {
        InitializeDungeon();
        PlaceEntrance();
        GenerateCriticalPath(_startPoint, _criticalPathLength, _criticalPathLength);
        ConnectExit();
        PrintDungeon();
    }

    private void InitializeDungeon()
    {
        for (int x = 0; x < _dimensions.X; x++)
        {
            _dungeon.Add([]);
            for (int y = 0; y < _dimensions.Y; y++)
            {
                _dungeon[x].Add(0);
            }
        }
    }

    private void PlaceEntrance()
    {
        if (_startPoint.X < 0 || _startPoint.X >= _dimensions.X)
            _startPoint.X = _random.Next(0, _dimensions.X - 1);
        if (_startPoint.Y < 0 || _startPoint.Y >= _dimensions.Y)
            _startPoint.Y = _random.Next(0, _dimensions.Y - 1);
        
        /*if (_endPoint.X < 0 || _endPoint.X >= _dimensions.X)
            _endPoint.X = _random.Next(0, _dimensions.X - 1);
        if (_endPoint.Y < 0 || _endPoint.Y >= _dimensions.Y)
            _endPoint.Y = _random.Next(0, _dimensions.Y - 1);*/
        
        _dungeon[_startPoint.X][_startPoint.Y] = "S";
        // _dungeon[_endPoint.X][_endPoint.Y] = "E";
    }
    
    private bool GenerateCriticalPath(Vector2I previousPoint, int length, object marker)
    {
        if (previousPoint == _endPoint)
            return true;
        
        if (length == 0)
            return true;
        
        Vector2I currentPoint = previousPoint;

        Vector2I direction = _random.Next(0, 3) switch
        {
            0 => Vector2I.Up,
            1 => Vector2I.Down,
            2 => Vector2I.Left,
            3 => Vector2I.Right,
            _ => throw new ArgumentOutOfRangeException()
        };

        for (int index = 0; index < 4; index++)
        {
            Vector2I nextPoint = previousPoint + direction;
            if (nextPoint.X >= 0 && nextPoint.X < _dimensions.X &&
                nextPoint.Y >= 0 && nextPoint.Y < _dimensions.Y &&
                (int)_dungeon[nextPoint.X][nextPoint.Y] == 0)
            {
                currentPoint += direction;
                _dungeon[currentPoint.X][currentPoint.Y] = (int)marker;
                
                if (GenerateCriticalPath(currentPoint, length - 1, length - 1))
                    return true;

                _dungeon[currentPoint.X][currentPoint.Y] = 0;
                currentPoint -= direction;
            }
            direction = new Vector2I(direction.Y, -direction.X);
        }
        return false;
    }

    private void ConnectExit()
    {
        if ((int)_dungeon[_endPoint.X][_endPoint.Y] == 0)
        {
            Vector2I currentPoint = FindClosestCriticalPoint(_endPoint);

            int xStep = _endPoint.X > currentPoint.X ? 1 : -1;
            for (int x = currentPoint.X; x != _endPoint.X; x += xStep)
                _dungeon[x][currentPoint.Y] = -1;
            
            int yStep = _endPoint.Y > currentPoint.Y ? 1 : -1;
            for (int y = currentPoint.Y; y != _endPoint.Y; y += yStep)
                _dungeon[currentPoint.X][y] = -1;
            
            _dungeon[_endPoint.X][_endPoint.Y] = "E";
        }
    }

    private Vector2I FindClosestCriticalPoint(Vector2I targetPoint)
    {
        Vector2I bestPoint = new();
        int bestDistance = int.MaxValue;

        for (int x = 0; x < _dimensions.X; x++)
        {
            for (int y = 0; y < _dimensions.Y; y++)
            {
                object cell = _dungeon[x][y];
                bool isCritical = false;
                
                switch (cell)
                {
                    case int intValue when intValue != 0:
                    case "S":
                        isCritical = true;
                        break;
                }

                if (isCritical)
                {
                    int distance = Math.Abs(x - targetPoint.X) + Math.Abs(y - targetPoint.Y);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestPoint = new Vector2I(x, y);
                    }
                }
            }
        }
        return bestPoint;
    }

    private void PrintDungeon()
    {
        string dungeonAsString = "";

        for (int y = _dimensions.Y - 1; y >= 0; y--)
        {
            for (int x = 0; x < _dimensions.X; x++)
            {
                dungeonAsString += $"[{_dungeon[x][y]}]";
                // if ((int)_dungeon[x][y] != 0 || (string)_dungeon[x][y] == "S" || (string)_dungeon[x][y] == "C")
                //     dungeonAsString += $"[{_dungeon[x][y]}]";
                //
                // else if ((int)_dungeon[x][y] == 0)
                //     dungeonAsString += $"   ";
            }
            dungeonAsString += '\n';
        }
        GD.Print(dungeonAsString);
    }
}