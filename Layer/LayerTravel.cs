using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerTravel : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int depthPrecision;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.sortingOrder = (int)(-transform.position.y * depthPrecision);
    }

}
