using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject playerPrefab;
    
    void Start()
    {
        for (int i = 1; i < 5; i++)
        {
            GameObject clone = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            clone.GetComponent<Player>().playerIndex = i.ToString();
        }
    }
}
