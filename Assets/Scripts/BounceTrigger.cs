using System;
using UnityEngine;

public class BounceTrigger : MonoBehaviour
{
    public float bounceHeight = 10;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.GetComponent<Rigidbody2D>().velocity.y < 5)
            return;

        Player p = collision.GetComponent<Player>();

        if (!p)
            return;
        var rb = p.GetComponent<Rigidbody2D>();

        p.coyoteJump = true;
        rb.velocity = new Vector2(rb.velocity.x, bounceHeight);
    }
    Vector3 p;
    private void Awake() {
        p = transform.position;
    }

    public float freq = 1, amp = .1f;

    private void Update() {
        var s = Mathf.Sin(Time.time * freq) * amp;

        transform.position = p + new Vector3(0, s, 0);
    }
}
