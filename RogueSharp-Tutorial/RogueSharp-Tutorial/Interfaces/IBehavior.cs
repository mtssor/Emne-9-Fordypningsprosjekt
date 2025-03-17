using RogueSharp_Tutorial.Core;
using RogueSharp_Tutorial.Systems;

namespace RogueSharp_Tutorial.Interfaces;

public interface IBehavior
{
    bool Act(Monster monster, CommandSystem commandSystem);
}