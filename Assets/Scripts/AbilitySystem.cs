using System;
using UnityEngine;


public enum AAbility
{
    DefaultJump, DoubleJump, Jetpack, BouncyShoes
}
public enum XAbility
{
    DefaultPunch, Sword, Axe, Hammer
}
public enum YAbility
{
    None, Missile, PickUp, FireShield, BubbleGun
}
public enum BAbility
{
    DefaultBlock, ReflectiveShield, Barrier, Evade
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
