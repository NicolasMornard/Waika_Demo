using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Animator UIAnimator;
    public bool ShowUIBar;
    public bool ShowUIItem;

    // Update is called once per frame
    void Update()
    {
        if (ShowUIBar)
        {
            UIAnimator.SetBool("BarOpen", true);
        } else
        {
            UIAnimator.SetBool("BarOpen", false);
        }
        if (ShowUIItem)
        {
            UIAnimator.SetBool("ItemOpen", true);
        } else
        {
            UIAnimator.SetBool("ItemOpen", false);
        }
    }
}
