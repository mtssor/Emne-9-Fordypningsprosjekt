using System;
using System.Collections.Generic;

namespace NewGameProject.Scripts.Systems.MapGeneration;

public class TreeConfig
{
    public int MaxDepth { get; set; }
    public int MaxChildren { get; set; }
    
    public Func<RoomNode, int, int> ChildrenCountRule { get; set; }
    public Func<RoomNode, int, IEnumerable<RoomTypes>> AllowedTypesRule { get; set; }
    public Func<IList<RoomNode>, RoomNode> ExitSelectorRule { get; set; }
}