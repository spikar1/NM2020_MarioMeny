using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayersPosition : MonoBehaviour
{
    GameObject[] players;
    public Transform[] transforms;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Player>().ResetPlayer();
            players[i].transform.position = transforms[i].position;

        }

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Player>().CheckIfStillDead();
        }  
    }
}
