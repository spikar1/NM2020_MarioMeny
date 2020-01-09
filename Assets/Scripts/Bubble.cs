using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private Rigidbody2D rb;
    public Player trappedPlayer;

    private float lifeTime = 0;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;

        if (lifeTime > Manager.WorldOptions.bubbleMaxLifetime)
        {
            BurstBubble();
        }
    }

    private void BurstBubble()
    {
        trappedPlayer.knockbackState = false;
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(rb.velocity.x, Manager.WorldOptions.bubbleSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            
        /*if(lifeTime > .6f)
        foreach (var point in collision.contacts)
        {
            if (point.normal.y < 0)
                BurstBubble();
        }*/
    }

    internal void Damage(Vector2 dir) {
        rb.velocity += dir * 20;
    }
}
