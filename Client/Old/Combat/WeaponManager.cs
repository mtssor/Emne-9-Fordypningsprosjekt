using System;
using Godot;

namespace NewGameProject.Old.Combat;

public partial class WeaponManager : Node
{
    public static void SetAnimation<T>(AnimationPlayer animationPlayer,
                                       T animationStatesEnum,
                                       float transitionTime = 0.1f,
                                       bool forceStop = false) where T : Enum
    {
        if (forceStop) animationPlayer?.Stop();
        string enumKey = animationStatesEnum.ToString();
        animationPlayer?.Play(enumKey, customBlend: transitionTime);
    }
}