using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

using Random = UnityEngine.Random;

public class Countdown : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    int min;
    float t;
    bool canInvoke;

    string minutes, seconds;
    public Vector2 startTime;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        min = (int)startTime.x;
        t = startTime.y;

        canInvoke = true;
    }


    // Update is called once per frame
    void Update()
    {
        if(t > 0)
        {
            t -= Time.deltaTime;
        }
        else if(min > 0)
        {
            min--;
            t = 60;
        }

        if((int)t < 10)
        {
            seconds = "0" + ((int)t).ToString();
        }
        else
        {
            seconds = ((int)t).ToString();
        }

        if(min < 10)
        {
            minutes = "0" + min.ToString();
        }
        else
        {
            minutes = min.ToString();
        }

        textMesh.text = $"{minutes}:{seconds}";


        if(min <= 0 && (int)t <= 0 && canInvoke)
        {
            canInvoke = false;
            SelectLevel();
        }
    }

    private void SelectLevel()
    {
        string levelName = "";
        string[] levelList = Manager.WorldOptions.levelList;
        if (levelList.Length <= 0)
            throw new Exception("World Options conains no levels");
        var r = Random.Range(0, levelList.Length);

        levelName = levelList[r];

        GameObject manager = GameObject.FindGameObjectWithTag("Manage");
        manager.GetComponent<Manager>().sceneLoader.LoadScene(levelName);
    }

    public void RushCountDown()
    {
        min = 0;
        t = 4;
    }
}
