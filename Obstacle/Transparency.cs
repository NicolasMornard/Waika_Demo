using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparency : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Color defaultColor;
    private Color fadedColor;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.7f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Avatar>())
        {
            FadeOut();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Avatar>())
        {
            FadeIn();
        }
    }

    public void FadeOut()
    {
        spriteRenderer.color = fadedColor;
    }

    public void FadeIn()
    {
        spriteRenderer.color = defaultColor;
    }
}
