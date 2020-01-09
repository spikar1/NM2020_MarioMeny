using System;
using UnityEngine;


public enum AAbility
{
    DefaultJump, DoubleJump, Jetpack
}
public enum XAbility
{
    DefaultPunch, Sword
}
public enum YAbility
{
    None, Blizzard, BubbleGun
}
public enum BAbility
{
    DefaultBlock, ReflectiveShield, Hammer
}

static public class AbilityExtensions
{
    static public Sprite GetIconA(this AAbility ability) {
        switch (ability) {
            case AAbility.DefaultJump:
                return Manager.WorldOptions.DefaultJumpSprite;
            case AAbility.DoubleJump:
                return Manager.WorldOptions.DefaultJumpSprite;
            default:
                throw new Exception("Missing sprite for " + ability.ToString());
        }

    }

}
