using System.Collections;
using UnityEngine;
using EZCameraShake;

public class FallLevel1 : SlideManager
{
    [Header("Earth Quake Attributes :")]
    public float magn = 1.0f;
    public float rough = 1.0f;
    public float fadeIn = 1.0f;
    public float fadeOut = 1.0f;
    [Header("Others :")]
    public float SlowingRatio = 0.4f;
    public float TimeBeforeSlide = 2.5f;

    private float speedKeeper;
    private bool disable;

    protected override void TriggerAction()
    {
        if (!disable) {
            StartCoroutine(level1Ending());
            disable = true;
        }
    }
    IEnumerator level1Ending()
    {
        CameraShaker.Instance.ShakeOnce(magn, rough, fadeIn, fadeOut);
        speedKeeper = GameDirector.Avatar.MaxSpeed;
        GameDirector.Avatar.MaxSpeed = speedKeeper * SlowingRatio;

        yield return new WaitForSeconds(TimeBeforeSlide);

        TriggerSlide();
        GameDirector.Avatar.MaxSpeed = speedKeeper;
    }
}
