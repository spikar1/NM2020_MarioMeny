using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleProjectile : Projectile
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        Player otherPlayer = collision.GetComponent<Player>();
        if (otherPlayer)
            Instantiate(Manager.worldOptions.bubblePrefab, otherPlayer.transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
