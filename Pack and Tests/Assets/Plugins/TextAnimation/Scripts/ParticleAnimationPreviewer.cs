using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ParticleAnimationPreviewer : MonoBehaviour
{
#if UNITY_EDITOR
    ParticleSystem pSystem;
    AnimationWindow animationWindow;
    private float previousFrameTime;

    private void Awake()
    {
        pSystem = GetComponent<ParticleSystem>();
        pSystem.useAutoRandomSeed = false;
        pSystem.randomSeed = (uint)UnityEngine.Random.Range(1, Int32.MaxValue);

        previousFrameTime = 0f;

    }

    void Update()
    {
        if (animationWindow == null) animationWindow = EditorWindow.GetWindow<AnimationWindow>();

        float time = animationWindow.time;
        if (time != previousFrameTime)
        {
            pSystem.Simulate(time, true, true);
        }

        previousFrameTime = time; 
    }
#endif
}
