using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRay : MonoBehaviour
{
    GameObject manager;
    StockCanvas stockCanvas;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manage");
        stockCanvas = GameObject.FindGameObjectWithTag("StockCanvas").GetComponent<StockCanvas>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            if (!player)
                return;

            if (player.stockCount == 1)
            {
                stockCanvas.UpdatePlayerStock(player.playerNumber, 0);
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
