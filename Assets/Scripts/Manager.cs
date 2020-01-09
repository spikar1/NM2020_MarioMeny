using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public WorldOptions _worldOptions;
    public SoundManager _soundManager;

    GameObject[] players;
    private int playersPlaying;
    int currentPlaying;
    public SceneLoader sceneLoader;

    public static bool debugMode = false;

    public static WorldOptions WorldOptions => Instance._worldOptions;

    static Manager instance = null;
    public static Manager Instance => instance;

    public static SoundManager SoundManager => Instance._soundManager;

    private void Awake() {

        if (!instance)
        {
            instance = this;
        }

        if (Debug.isDebugBuild)
            debugMode = true;
        else
            debugMode = false;
        

        if (Instance != this)
            Destroy(this);

        DontDestroyOnLoad(gameObject);

        sceneLoader = GetComponent<SceneLoader>();
    }


    public void CheckRemainingPlayers()
    {
        playersPlaying = 0;
        currentPlaying = 0;

        players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            playersPlaying++;

            if(!players[i].GetComponent<Player>().playerIsDead)
            {
                currentPlaying++;
            }
        }

        if(playersPlaying > 1)
        {
            if(currentPlaying < 2)
            {
                //Go to SelectionScreen:
                sceneLoader.LoadScene("Selection Screen");
            }
        }
        else
        {
            //Go To Victory Screen:
            sceneLoader.LoadScene("WinScreen");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            debugMode = !debugMode;
        }
    }

    private void OnGUI()
    {
        if (!debugMode)
            return;

        Rect buttonPos = new Rect(50, 100, 50, 50);

        
    }
}
