using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IBumpable
{
    #region PlayerStats
    public float maxSpeed = 1;
    public float jumpHeight = 1;
    public float acceleration = 1;
    #endregion

    Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        float horInput = 0;
        bool jumpInput = Input.GetButtonDown("Jump");

        //TODO: Needs support for multiple players
        horInput = Input.GetAxisRaw("Horizontal");

        Move(horInput);
        if (jumpInput) {
            Jump();
        }
    }

    private void Move(float direction) {
        rb.velocity = new Vector2(direction * maxSpeed, rb.velocity.y);
    }

    private void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    public void Bumped(Player other) {
        print(name + " is bumped by " + other);
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
