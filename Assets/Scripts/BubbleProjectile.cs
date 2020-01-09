using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleProjectile : Projectile
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        Bubble bubble = collision.GetComponent<Bubble>();
        if (bubble)
            Destroy(gameObject);

        Player otherPlayer = collision.GetComponent<Player>();
        if (!otherPlayer)
            otherPlayer = collision.transform.parent.GetComponent<Player>();

        if (otherPlayer)
            Instantiate(Manager.WorldOptions.bubblePrefab, otherPlayer.transform.position + Vector3.up * .5f, Quaternion.identity);

        Destroy(gameObject);
    }
}
