using System;
using System.Collections.Generic;
using System.Linq;

namespace NewGameProject.Scripts.Systems.MapGeneration;

public class Tree(TreeConfig config, int? seed = null)
{
    private readonly Random _random = seed.HasValue ? new Random(seed.Value) : new Random();
    private readonly TreeConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    public RoomNode GenerateTree()
    {
        RoomNode root = new(RoomTypes.Entrance);
        BuildChildren(root, 1);

        List<RoomNode> leaves = GetLeaves(root);
        if (leaves.Count > 0)
        {
            RoomNode exitNode = _config.ExitSelectorRule(leaves);
            exitNode.Value = RoomTypes.Exit;
        }
        return root;
    }

    private void BuildChildren(RoomNode node, int currentDepth)
    {
        if (currentDepth >= _config.MaxDepth)
            return;
        
        int childCount = _config.ChildrenCountRule?.Invoke(node, currentDepth) 
                         ?? _random.Next(1, _config.MaxDepth + 1);
        
        for (int i = 0; i < childCount; i++)
        {
            IEnumerable<RoomTypes> allowedTypes = _config.AllowedTypesRule?.Invoke(node, currentDepth) 
                                                  ?? [RoomTypes.Normal];

            RoomTypes[] typesArray = (RoomTypes[])allowedTypes ?? allowedTypes.ToArray();
            RoomTypes chosenType = typesArray[_random.Next(typesArray.Length)];
            
            RoomNode childNode = new(chosenType);
            node.AddChild(childNode);
            BuildChildren(childNode, currentDepth + 1);
        }
    }
    
    private List<RoomNode> GetLeaves(RoomNode node)
    {
        List<RoomNode> leaves = [];
        if (node.Children.Count == 0)
            leaves.Add(node);
        else
        {
            foreach (RoomNode child in node.Children)
                leaves.AddRange(GetLeaves(child));
        }
        return leaves;
    }
}