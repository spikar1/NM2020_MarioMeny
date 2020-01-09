using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WorldOptions", menuName ="MarioMenu/New World Options")]
public class WorldOptions : ScriptableObject
{
    [Header("General Options")]
    public string[] levelList = new string[] { "level_1" };
    public bool playersCanJumpAfterBouncePad = true;
    public bool bounceGameplay = true;

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
    public bool useAcceleration = true;
    public float maxSpeed = 7;
    public float acceleration = 1;
    public float deaccelerationSpeed = 1;
    public float iceFloorAcceleration = .4f;
    public float jumpHeight = 10;
    public int stockCount = 5;
    public float punchDistance = 15;
    public float KnockbackStartAmount = 10;
    public float blockedKnockback = 0.2f;
    public float knockbackScaler = 2;
    public float bouncyKnockback = 10;

    [Header("Player Colors")]
    public Color[] playerColors;


    [Header("X Ability Cooldowns")]
    [Header("Cooldowns")]
    public float defaultPunchCooldownTime = 1;
    public float swordCooldownTime = 1;
    public float hammerCooldownTime = 1;
    public float axeCooldownTime = 1;

    [Header("Y Ability Cooldowns")]
    public float missileCooldownTime = 3;
    public float bubbleGunCooldownTime = 3;
    internal float blizzardCooldown = 10;

    [Header("Ability parameters")]
    public int missileSpeed = 6;
    public int maxSwordCharge = 10;
    public float bubbleSpeed;
    public float bubbleMaxLifetime;
    public AnimationCurve magicMissileYCurve;
    public float magicMissileHomingSpeed;
    public float magicMissileMaxSpeed;
    public float MaxJetpackDuration = 2;
    public float bubbleGunProjectileSpeed;
    [Tooltip("Seconds the blizzards lasts for")]
    public float blizzardDuration = 3;
    [Range(0, 1)]
    public float shieldVelocityCutoff = .4f;

    public List<AAbility> availableAAbilities = new List<AAbility>();
    public List<XAbility> availableXAbilities = new List<XAbility>();
    public List<YAbility> availableYAbilities = new List<YAbility>();
    public List<BAbility> availableBAbilities = new List<BAbility>();
}
