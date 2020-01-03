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
    bool jumpInput;
    #endregion

    Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate() {
        Move(horInput);
    }

    private void Update() {
        GetInputs();
        if (jumpInput) {
            Jump();
        }
    }

    void GetInputs() {
        jumpInput = Input.GetButtonDown("Jump_P"+1);

        //TODO: Needs support for multiple players
        horInput = Input.GetAxisRaw("Horizontal_P" + playerIndex);
    }

    private void Move(float direction) {
        rb.velocity = new Vector2(direction * maxSpeed, rb.velocity.y);
    }

    private void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    public void Bumped(Player other) {
        print(name + " is bumped by " + other);
        Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        Transform other = collision.collider.transform;
        float otherY = other.position.y;
        IBumpable bumpable = other.GetComponent<IBumpable>();
        if (bumpable == null)
            return;


        if(otherY > transform.position.y && rb.velocity.y > 0) {
            bumpable.Bumped(this);
        }
    }
}
