using System;
using UnityEngine;

public class BounceTrigger : MonoBehaviour
{
    public float bounceHeight = 10;

    private void OnTriggerStay2D(Collider2D collision) {
        Player p = collision.GetComponent<Player>();

        if (!p)
            return;
        var rb = p.GetComponent<Rigidbody2D>();

        p.coyoteJump = true;
        rb.velocity = new Vector2(rb.velocity.x, bounceHeight);
    }
}
