using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        Player otherPlayer = collision.GetComponent<Player>();
        if (otherPlayer)
            otherPlayer.Damage(new Vector2(dir, 0));

        Destroy(gameObject);
    }
}
