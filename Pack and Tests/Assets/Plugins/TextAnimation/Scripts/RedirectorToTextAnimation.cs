using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RedirectorToTextAnimation : MonoBehaviour
{
    [SerializeField] TextAnimator textAnimator;

    public void ChangeTextAnimation(TextAnimationScriptable newTextAnim)
    {
        textAnimator.ChangeTextAnimation(newTextAnim);
    }
}
