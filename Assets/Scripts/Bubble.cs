using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private Rigidbody2D rb;

    private float lifeTime = 0;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;

        if (lifeTime > Manager.worldOptions.bubbleMaxLifetime)
        {
            BurstBubble();
        }
    }

    private void BurstBubble()
    {
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector2.up * Manager.worldOptions.bubbleSpeed * Time.fixedDeltaTime);
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
}
