using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    void Update()
    {
        transform.position -= new Vector3(0, 25, 0) * Time.deltaTime;
    }
}
