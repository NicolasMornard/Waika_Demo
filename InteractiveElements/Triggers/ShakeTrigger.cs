using UnityEngine;
using EZCameraShake;
using System.Collections;

public class ShakeTrigger : ElementTrigger
{
    [Header("ShakeOnce")]
    public bool EnableShakeOnce;
    [ConditionalHide("EnableShakeOnce", false)]
    public float Magn = 5.0f;
    [ConditionalHide("EnableShakeOnce", false)]
    public float Rough = 5.0f;
    [ConditionalHide("EnableShakeOnce", false)]
    public float FadeIn = 3.0f;
    [ConditionalHide("EnableShakeOnce", false)]
    public float FadeOut = 3.0f;

    [Header("StartShake")]
    public bool EnbaleStartShake;
    [ConditionalHide("EnbaleStartShake", false)]
    public float Magn1 = 5.0f;
    [ConditionalHide("EnbaleStartShake", false)]
    public float Rough1 = 5.0f;
    [ConditionalHide("EnbaleStartShake", false)]
    public float FadeIn1 = 2.0f;
    [ConditionalHide("EnbaleStartShake", false)]
    public float Duration = 10.0f;
    [ConditionalHide("EnbaleStartShake", false)]
    public float FadeOut1 = 2.0f;

    private CameraShakeInstance cs;

    protected override void TriggerAction()
    {
        if (EnableShakeOnce)
        {
            CameraShaker.Instance.ShakeOnce(Magn, Rough, FadeIn, FadeOut);
        }
        else if (EnbaleStartShake)
        {
            StartCoroutine(StartShake());
        }
    }
    IEnumerator StartShake()
    {
        cs = CameraShaker.Instance.StartShake(Magn1, Rough1, FadeIn1);
        if (Duration != 0)
        {
            yield return new WaitForSeconds(Duration);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }
        cs.StartFadeOut(FadeOut1);


    }
}
