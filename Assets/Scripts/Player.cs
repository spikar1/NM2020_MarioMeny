using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IBumpable
{
    public string playerIndex = "1";

    #region PlayerStats
    public float maxSpeed = 7;
    public float jumpHeight = 10;
    public float acceleration = 1;
    public int stockCount = 3;
    #endregion

    #region Inputs
    float horizontalInput;
    #endregion

    #region Abilities
    AAbility aAbility = AAbility.DefaultJump;
    XAbility xAbility = XAbility.DefaultPunch;
    YAbility yAbility = YAbility.None;
    BAbility bAbility = BAbility.DefaultBlock;
    #endregion

    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Animator anim;

    private void Awake() {
        anim = gameObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    public void ResetPlayer() {
        aAbility = Manager.worldOptions.aDefault;
        xAbility = Manager.worldOptions.xDefault;
        yAbility = Manager.worldOptions.yDefault;
        bAbility = Manager.worldOptions.bDefault;
    }

    private void Start() {

    }
    void FixedUpdate() {
        Move(horizontalInput);
    }

    int dir = 1;

    private void Update() {
        horizontalInput = Input.GetAxisRaw("Horizontal_P" + playerIndex);

        if (Input.GetButtonDown("A" + "_P" + playerIndex)) {
            DoAAbility();
        }
        //TODO: Add rest of buttons

        //TODO: Reiterate upon turning method.
        if(horizontalInput != 0) {
            dir = (int)Mathf.Sign(horizontalInput);
        }
        transform.localScale = new Vector3(
            dir, 
            transform.localScale.y, 
            transform.localScale.z);
    }



    void DoAAbility() {
        switch (aAbility) {
            case AAbility.DefaultJump:
                Jump();
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

    private void Move(float direction) {
        rb.velocity = new Vector2(direction * maxSpeed, rb.velocity.y);
    }

    public void Bumped(Player other, Vector2 collisionVector) {
        print(name + " is bumped by " + other);
        //TODO: Add bump method
    }

    void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
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
