using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeColorCoroutine(Image image, Color startColor, Color endColor, float duration, float deltaTime)
    {
        Debug.Log("Test");
        fps = 1.0f / Time.deltaTime;
        float colorSpeed = (1.0f / duration) / fps;
        timer += colorSpeed;
        image.color = Color.Lerp(startColor, endColor, timer);
        Debug.Log(timer);
        yield return new WaitForSeconds(colorSpeed);
        FadeColorCoroutine(image, startColor, endColor, duration, deltaTime);
    }

    public void FadeColor(Image image, Color startColor, Color endColor, float duration, float deltaTime)
    {
        StartCoroutine(FadeColorCoroutine(image, startColor, endColor, duration, deltaTime));
    }

    private float timer = 0.0f;
    private float fps;
}
