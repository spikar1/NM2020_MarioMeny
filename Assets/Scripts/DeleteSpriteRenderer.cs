using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSpriteRenderer : MonoBehaviour
{

    void Start()
    {
        Destroy(GetComponent<SpriteRenderer>());
    }

}
