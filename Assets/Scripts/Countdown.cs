using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

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
            GameObject manager = GameObject.FindGameObjectWithTag("Manage");
            manager.GetComponent<Manager>().sceneLoader.LoadScene(1);
        }
    }
}
