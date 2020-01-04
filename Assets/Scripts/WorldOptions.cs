using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WorldOptions", menuName ="MarioMenu/New World Options")]
public class WorldOptions : ScriptableObject
{
    public AAbility aDefault = AAbility.DefaultJump;
    public XAbility xDefault = XAbility.DefaultPunch;
    public YAbility yDefault = YAbility.None;
    public BAbility bDefault = BAbility.DefaultBlock;

    public Sprite DefaultJumpSprite;
}
