using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSprite : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;

    SpriteRenderer sr;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = sprites.Length > 0 ? sprites[Random.Range(0, sprites.Length)] : sr.sprite;
    }
}
