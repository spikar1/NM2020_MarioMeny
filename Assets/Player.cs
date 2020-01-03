using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region PlayerStats
    public float maxSpeed = 1;
    public float jumpHeight = 1;
    public float acceleration = 1;
    #endregion

    Rigidbody2D rb;


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

    }

    private void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }
}
