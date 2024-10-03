using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextAnimation", menuName = "ScriptableObjects/TextAnimation")]
public class TextAnimationScriptable : ScriptableObject
{
    public AnimationCurve OffsetX;
    public AnimationCurve OffsetY;
    public AnimationCurve ScaleX;
    public AnimationCurve ScaleY;
    public AnimationCurve Angle;
    public AnimationCurve Alpha;
}
