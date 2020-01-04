using System;
using System.Collections;
using System.Collections.Generic;
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

public class Player : MonoBehaviour, IBumpable
{
    #region PlayerStats
    public float maxSpeed = 7;
    public float jumpHeight = 10;
    public float acceleration = 1;
    public int stockCount = 3;
    #endregion

    #region Inputs
    public string playerIndex = "1";
    float horInput;
    #endregion

    #region Abilities
    //public AbilityAsset aAbility;
    //public AbilityAsset xAbility;
    //public AbilityAsset yAbility;
    //public AbilityAsset bAbility;
    AAbility aAbility = AAbility.DefaultJump;
    XAbility xAbility = XAbility.DefaultPunch;
    YAbility yAbility = YAbility.None;
    BAbility bAbility = BAbility.DefaultBlock;
    #endregion

    #region Slots
    public  Transform rightHand, leftHand;
    #endregion

    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Animator anim;

    private void Awake() {
        anim = gameObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        /*
        if(aAbility)
            aAbility.player = this;
        if(xAbility)
            xAbility.player = this;
        if(yAbility)
            yAbility.player = this;
        if(bAbility)
            bAbility.player = this;*/
    }

    private void Start() {
        /*aAbility = aAbility == null ? Manager.worldOptions.aDefault : aAbility;
        xAbility = xAbility == null ? Manager.worldOptions.xDefault : xAbility;
        yAbility = yAbility == null ? Manager.worldOptions.yDefault : yAbility;
        bAbility = bAbility == null ? Manager.worldOptions.bDefault : bAbility;*/
    }
    void FixedUpdate() {
        Move(horInput);
    }

    int dir = 1;

    private void Update() {
        GetInputs();
        SendInputs("A");
        SendInputs("X");
        SendInputs("Y");
        SendInputs("B");



        if(horInput != 0) {
            dir = (int)Mathf.Sign(horInput);
        }
        transform.localScale = new Vector3(
            dir, 
            transform.localScale.y, 
            transform.localScale.z);
    }

    private void SendInputs(string buttonString) {
        string button = buttonString + "_P" + playerIndex;
        //button = "Jump_P1"; //DEBUG!!!



        if (Input.GetButtonDown(button)) {

            DoAAbilty();
        }
        if (Input.GetButtonUp(button)) {

        }
        if (Input.GetButton(button)) {

        }
    }

    void DoAAbilty() {
        switch (aAbility) {
            case AAbility.DefaultJump:
                break;
            case AAbility.DoubleJump:
                break;
            case AAbility.Jetpack:
                break;
            case AAbility.BouncyShoes:
                break;
            default:
                break;
        }
    }
    void DoXAbility() {
        switch (bAbility) {
            case BAbility.DefaultBlock:
                break;
            case BAbility.ReflectiveShield:
                break;
            case BAbility.Barrier:
                break;
            case BAbility.Evade:
                break;
            default:
                break;
        }
    }
    void DoYAbility() {
        switch (yAbility) {
            case YAbility.None:
                break;
            case YAbility.Missile:
                break;
            case YAbility.PickUp:
                break;
            case YAbility.FireShield:
                break;
            case YAbility.BubbleGun:
                break;
            default:
                break;
        }
    }
    void DoBAbility() {
        switch (bAbility) {
            case BAbility.DefaultBlock:
                break;
            case BAbility.ReflectiveShield:
                break;
            case BAbility.Barrier:
                break;
            case BAbility.Evade:
                break;
            default:
                break;
        }
    }

    void GetInputs() {
        //jumpInput = Input.GetButtonDown("Jump_P"+1);

        //TODO: Needs support for multiple players
        horInput = Input.GetAxisRaw("Horizontal_P" + playerIndex);
    }

    private void Move(float direction) {
        rb.velocity = new Vector2(direction * maxSpeed, rb.velocity.y);
    }

    public void Bumped(Player other, Vector2 collisionVector) {
        print(name + " is bumped by " + other);
    }

    private void OnCollisionEnter2D(Collision2D collision) {


        Transform other = collision.collider.transform;
        float otherY = other.position.y;
        IBumpable bumpable = other.GetComponent<IBumpable>();
        if (bumpable == null)
            return;


        if(otherY > transform.position.y && collision.GetContact(0).normal.y < 0) {
            bumpable.Bumped(this, collision.relativeVelocity);          
        }
    }

    public void LooseStock()
    {
        stockCount--;
    }
}
