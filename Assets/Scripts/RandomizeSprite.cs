using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSprite : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;

    SpriteRenderer sr;
    static float offset = 0;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = sprites.Length > 0 ? sprites[Random.Range(0, sprites.Length)] : sr.sprite;
        transform.position -= new Vector3(0, 0, offset);
        sr.flipX = Random.value > .5f;
        offset += .0001f;
    }
}
