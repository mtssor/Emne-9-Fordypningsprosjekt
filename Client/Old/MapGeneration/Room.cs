using Godot;

namespace NewGameProject.Old.MapGeneration;



public class Room
{
    public Vector2I Position { get; init; }
    public Vector2I Size { get; set; }
    
    public RoomTypes RoomType { get; set; }
}