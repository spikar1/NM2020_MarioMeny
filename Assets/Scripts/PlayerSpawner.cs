using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject playerPrefab;
    int playerCount;
    string stick1, stick2, stick3, stick4;
    KeyCode kc1, kc2, kc3, kc4;
    bool pla1, pla2, pla3, pla4;
    private bool plaDebug;

    private void Awake()
    {
        stick1 = $"Joystick{1}Button7";
        kc1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), stick1);
        pla1 = true;

        stick2 = $"Joystick{2}Button7";
        kc2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), stick2);
        pla2 = true;

        stick3 = $"Joystick{3}Button7";
        kc3 = (KeyCode)System.Enum.Parse(typeof(KeyCode), stick3);
        pla3 = true;

        stick4 = $"Joystick{4}Button7";
        kc4 = (KeyCode)System.Enum.Parse(typeof(KeyCode), stick4);
        pla4 = true;
        
        plaDebug = true;
    }

    private void Update()
    {
        if(Input.GetButtonDown("START") && playerCount < 4)
        {

            if(Input.GetKeyDown(kc1) && pla1)
            {
                pla1 = false;
                playerCount++;
                print("Player 1");
                SpawnPlayer(1, playerCount);
            }           

            if (Input.GetKeyDown(kc2) && pla2)
            {
                pla2 = false;
                playerCount++;
                print("Player 2");
                SpawnPlayer(2, playerCount);
            }

            if (Input.GetKeyDown(kc3) && pla3)
            {
                pla3 = false;
                playerCount++;
                print("Player 3");
                SpawnPlayer(3, playerCount);
            }

            if (Input.GetKeyDown(kc4) && pla4)
            {
                pla4 = false;
                playerCount++;
                print("Player 4");
                SpawnPlayer(4, playerCount);
            }
        }
    }

    void SpawnPlayer(int index, int number)
    {
        GameObject clone = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        clone.GetComponent<Player>().playerIndex = index.ToString();
        clone.GetComponent<Player>().playerNumber = number;
    }

    private void OnGUI()
    {
        if (!Manager.debugMode)
            return;

        Rect buttonPos = new Rect(50, 50, 50, 50);

        if (GUI.Button(buttonPos, "SpawnPlayer"))
        {
            print("Debug Player");
            plaDebug = false;
            playerCount++;
            SpawnPlayer(1, playerCount);
        }
    }
}
