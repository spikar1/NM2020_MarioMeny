using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    GameObject manager;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manage");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            if (!player)
                return;

            if(player.stockCount == 1)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                player.LooseStock();
            }

            StartCoroutine(WaitBeforeChecking());
                    
        }
    }

    IEnumerator WaitBeforeChecking()
    {
        yield return new WaitForSeconds(1);
        manager.GetComponent<Manager>().CheckRemainingPlayers();
    }
}
