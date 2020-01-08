using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleProjectile : Projectile
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        Player otherPlayer = collision.GetComponent<Player>();
        if (otherPlayer)
            Instantiate(Manager.WorldOptions.bubblePrefab, otherPlayer.transform.position + Vector3.up * .5f, Quaternion.identity);

        Destroy(gameObject);
    }
}
