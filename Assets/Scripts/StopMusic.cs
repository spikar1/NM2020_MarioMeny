using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMusic : MonoBehaviour
{
    static bool hasStarted = false;
    private void Awake() {
        if (!hasStarted) {
            GetComponent<AudioSource>().Play();
            hasStarted = true;
        }
    }
}
