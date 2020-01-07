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

    [Header("PlayerStats")]
    public float maxSpeed = 7;
    public float jumpHeight = 10;
    public int stockCount = 5;
    public float punchDistance = 15;
    public float KnockbackStartAmount = 10;

    [Header("Player Colors")]
    public Color[] playerColors;


    [Header("Y Ability Cooldowns")]
    [Header("Cooldowns")]
    public float missileCooldownTime = 3;
    public float bubbleGunCooldownTime = 3;


    [Header("Ability parameters")]
    public int missileSpeed = 6;
    public int maxSwordCharge = 10;
    public float bubbleSpeed;
    public float bubbleMaxLifetime;
    public float MaxJetpackDuration = 2;
}
