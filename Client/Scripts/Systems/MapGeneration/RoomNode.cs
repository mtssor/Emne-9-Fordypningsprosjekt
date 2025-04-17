using System.Collections.Generic;

namespace NewGameProject.Scripts.Systems.MapGeneration;

public enum RoomTypes
{
    Entrance,
    Normal,
    Exit
}
public class RoomNode(RoomTypes value)
{
    public RoomTypes Value { get; set; } = value;
    public RoomNode Parent { get; set; }
    public List<RoomNode> Children { get; set; } = [];
    
    public void AddChild(RoomNode childRoom)
    {
        childRoom.Parent = this;
        Children.Add(childRoom);
    }
}