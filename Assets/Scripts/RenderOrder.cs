using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RenderOrder : MonoBehaviour
{
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        // SPRITE RENDERING
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }
}

