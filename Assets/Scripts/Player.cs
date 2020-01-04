using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IBumpable
{
    #region PlayerStats
    public float maxSpeed = 7;
    public float jumpHeight = 10;
    public float acceleration = 1;
    #endregion

    #region Inputs
    public string playerIndex = "1";
    float horInput;
    #endregion

    #region Abilities
    public AbilityAsset aAbility;
    public AbilityAsset xAbility;
    public AbilityAsset yAbility;
    public AbilityAsset bAbility;
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
        aAbility = aAbility == null ? Manager.worldOptions.aDefault : aAbility;
        xAbility = xAbility == null ? Manager.worldOptions.xDefault : xAbility;
        yAbility = yAbility == null ? Manager.worldOptions.yDefault : yAbility;
        bAbility = bAbility == null ? Manager.worldOptions.bDefault : bAbility;
    }
    void FixedUpdate() {
        Move(horInput);
    }

    int dir = 1;

    private void Update() {
        GetInputs();
        SendInputs("A", aAbility);
        SendInputs("X", xAbility);
        SendInputs("Y", yAbility);
        SendInputs("B", bAbility);

        if(horInput != 0) {
            dir = (int)Mathf.Sign(horInput);
        }
        transform.localScale = new Vector3(
            dir, transform.localScale.y, transform.localScale.z);
    }

    private void SendInputs(string buttonString, AbilityAsset ability) {
        string button = buttonString + "_P" + playerIndex;
        //button = "Jump_P1"; //DEBUG!!!

        if (Input.GetButtonDown(button)) {
            print(button + " was pressed");
            ability.ButtonDown(this);

        }
        if (Input.GetButtonUp(button))
            ability.ButtonUp(this);
        if (Input.GetButton(button))
            ability.ButtonHeld(this);
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

}
