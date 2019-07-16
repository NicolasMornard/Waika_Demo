using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Color Fader;
    public Color StartColor;
    public Color EndColor;
    public float Duration;
    public Image StartLogo;

    // Start is called before the first frame update
    void Start()
    {
        uiAnimator = GetComponent<UIAnimator>();
        uiAnimator.FadeColor(StartLogo, StartColor, EndColor, Duration, Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {
    }

    private UIAnimator uiAnimator;
}
