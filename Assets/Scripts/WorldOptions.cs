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

    [Header("Prefabs")]
    public GameObject missilePrefab;
    public GameObject bubbleProjectilePrefab;
    public GameObject bubblePrefab;

    [Header("Y Ability Cooldowns")]
    [Header("Cooldowns")]
    public float missileCooldownTime = 3;
    public float bubbleGunCooldownTime = 3;

    [Header("Ability parameters")]
    public int missileSpeed = 6;
    public float bubbleSpeed;
    public float bubbleMaxLifetime;
}
