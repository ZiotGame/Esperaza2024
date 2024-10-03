using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Mathematics;
//using System.Xml.Schema;
//using UnityEngine.Rendering;
//using Unity.VisualScripting;

[ExecuteAlways]
public class TextAnimator : MonoBehaviour
{
    [SerializeField] TextAnimationScriptable textAnimationScriptable;
    [SerializeField, Range(0f, 1f)] float progress;
    [SerializeField, Range(0f, 1f)] float sync;

    [SerializeField] bool useRadialPosition;

    TMP_Text textComponent;

    public void ChangeTextAnimation(TextAnimationScriptable newTextAnim)
    {
        textAnimationScriptable = newTextAnim;
    }

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (progress == 1f) return;

        AnimationCurve OffsetX = textAnimationScriptable.OffsetX;
        AnimationCurve OffsetY = textAnimationScriptable.OffsetY;
        AnimationCurve ScaleX = textAnimationScriptable.ScaleX;
        AnimationCurve ScaleY = textAnimationScriptable.ScaleY;
        AnimationCurve Angle = textAnimationScriptable.Angle;
        AnimationCurve Alpha = textAnimationScriptable.Alpha;

        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible) continue;

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            var colors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;

            //Calculate the normalized time for sampling curves for each character
            float nTime = progress;
            if (sync != 1f)
            {
                float nIn = ((float)i + 0.5f) / (float)textInfo.characterCount;  
                float syncWidth = 1f / Mathf.Tan((1f - sync) * Mathf.PI * 0.5f); //maps [0;1] to [0;+inf]
                float pMin = Mathf.Lerp(-syncWidth, 1f, progress);
                float pMax = Mathf.Lerp(0f, 1f + syncWidth, progress);
                nTime = (nIn - pMin) / Mathf.Max((pMax - pMin), 1e-30f);
                nTime = 1f - Mathf.Clamp01(nTime);
            }

            Vector3 midPoint = (charInfo.vertex_BL.position + charInfo.vertex_TL.position + charInfo.vertex_BR.position + charInfo.vertex_TR.position) * 0.25f;

            for (int j = 0; j < 4; ++j)
            {
                int index = charInfo.vertexIndex + j;
                var origPos = verts[index];

                if (!useRadialPosition)
                {

                    Vector3 offset = new Vector3(OffsetX.Evaluate(nTime),
                                                 OffsetY.Evaluate(nTime),
                                                 0);

                    Vector3 scale = new Vector3(ScaleX.Evaluate(nTime),
                                                ScaleY.Evaluate(nTime),
                                                0);
                    verts[index] = Quaternion.AngleAxis(Angle.Evaluate(nTime), Vector3.forward) * (origPos - midPoint);
                    verts[index] = Vector3.Scale(verts[index], scale) + midPoint + offset;
                }
                else
                {
                    
                    Vector3 scale = new Vector3(ScaleX.Evaluate(nTime),
                                                ScaleY.Evaluate(nTime),
                                                0);

                    Vector3 dir = Vector3.Normalize(midPoint);
                    float a = Vector3.SignedAngle(dir, Vector3.right, Vector3.forward);

                    verts[index] = Quaternion.AngleAxis(a, Vector3.forward) * (origPos - midPoint);
                    verts[index] = Vector3.Scale(verts[index], scale);

                    Vector3 midPoint2 = Vector3.LerpUnclamped(Vector3.zero, midPoint, OffsetX.Evaluate(nTime));
                    verts[index] = Quaternion.AngleAxis(-a, Vector3.forward) * verts[index] + midPoint2;
                }

                var origColor = colors[index];
                byte newAlpha = Convert.ToByte(origColor.a * Mathf.Clamp01(Alpha.Evaluate(nTime)));
                colors[index] = new Color32(origColor.r, origColor.g, origColor.b, newAlpha);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            meshInfo.mesh.colors32 = meshInfo.colors32;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
