using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    internal int dir = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();    
    }


    public void Initialize(int dir)
    {
        rb.velocity = new Vector2(dir * Manager.worldOptions.missileSpeed, 0);
        this.dir = dir;
    }

    abstract public void OnTriggerEnter2D(Collider2D collision);

}
