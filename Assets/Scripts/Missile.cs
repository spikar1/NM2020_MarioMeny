using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    private int dir = 1;

    public void Initialize(int dir)
    {
        rb.velocity = new Vector2(dir * Manager.worldOptions.missileSpeed, 0);
        this.dir = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player otherPlayer = collision.GetComponent<Player>();
        if (otherPlayer)
            otherPlayer.Damage(new Vector2(dir, 0));
            

        Destroy(gameObject);
    }
}
