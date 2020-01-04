using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    int min;
    int sec;

    public Vector2 startTime;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        min = (int)startTime.x;
        sec = (int)startTime.y;
    }

    float t = 0;

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        sec = (int)t;

        if(sec > 0)
        {
            //sec -= Time.deltaTime;
        }

        textMesh.text = $"{min}:{sec}";
    }
}
