using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSelection : MonoBehaviour
{
    GameObject[] players;
    int playersAlive, playersDead;
    bool haveChecked;
    public Countdown countDown;

    void Start()
    {
        checkAlivePlayers();
        haveChecked = false;
    }

    void Update()
    {
        if(playersAlive == playersDead && !haveChecked)
        {
            haveChecked = true;
            countDown.RushCountDown();
        }
    }

    public void checkDeadPlayers()
    {
        playersDead = 0;

        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            var player = players[i].GetComponent<Player>();

            if(player.playerIsDead)
            {
                playersDead++;
            }
        }
    }

    public void checkAlivePlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            playersAlive++;
        }
    }
}
