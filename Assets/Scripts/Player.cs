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

    #region InputRelated
    float horizontalInput;
    public bool isattacking;
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

        DontDestroyOnLoad(gameObject);
    }

    public void ResetPlayer() {
        aAbility = Manager.worldOptions.aDefault;
        xAbility = Manager.worldOptions.xDefault;
        yAbility = Manager.worldOptions.yDefault;
        bAbility = Manager.worldOptions.bDefault;
    }

    private void Start() {
        stockCount = 3;
    }

    void FixedUpdate() {
        if (knockbackState) {
            if (rb.velocity.magnitude < exitKnockbackMagnitude)
                knockbackState = false;
            return;
        }

        Move(horizontalInput);
    }

    int dir = 1;
    private bool knockbackState;
    public float exitKnockbackMagnitude = 3f;
    [SerializeField]
    private float knockbackAmount = 20;  

    private void Update() {
        if (knockbackState) {
            
            return;
        }
        horizontalInput = Input.GetAxisRaw("Horizontal_P" + playerIndex);

        if (Input.GetButtonDown("A" + "_P" + playerIndex)) {
            DoAAbility();
        }
        if (Input.GetButtonDown("X" + "_P" + playerIndex)) {
            DoXAbility();
        }
        //TODO: Add rest of buttons

        //TODO: Reiterate upon turning method.
        if(horizontalInput != 0) {
            dir = (int)Mathf.Sign(horizontalInput);
        }
        if(!isattacking)
            transform.localScale = new Vector3(
                dir, 
                transform.localScale.y, 
                transform.localScale.z);
    }

    public void StopAbility(string abilityString) {
        anim.SetBool(abilityString, false);
        isattacking = false;
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
        isattacking = true;
        switch (xAbility) {
            case XAbility.DefaultPunch:
                anim.SetBool("DefaultAttack", true);
                break;
            case XAbility.Sword:
                break;
            case XAbility.Axe:
                break;
            case XAbility.Hammer:
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
        if (isattacking) {
            var player = collision.collider.GetComponent<Player>();
            if(player) {
                print("Has connected attack");
                player.Damage((transform.position - player.transform.position).normalized);
            } 
        }

        Transform other = collision.collider.transform;
        float otherY = other.position.y;
        IBumpable bumpable = other.GetComponent<IBumpable>();
        if (bumpable == null)
            return;


        if(otherY > transform.position.y && collision.GetContact(0).normal.y < 0) {
            bumpable.Bumped(this, collision.relativeVelocity);          
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (!isattacking)
            return;
        var player = col.GetComponent<Player>();
        if (!player)
            return;

        player.Damage((player.transform.position - transform.position).normalized);
    }

    private void Damage(Vector2 dir) {
        print("Trying to damage");

        rb.velocity = dir * knockbackAmount + Vector2.up;
        //rb.velocity += Vector2.up * 100;
        knockbackState = true;
    }

    public void LooseStock()
    {
        print($"{playerIndex} Lost a Stock!");
        stockCount--;
    }
}
