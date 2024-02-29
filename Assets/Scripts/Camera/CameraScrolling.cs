using Cinemachine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public class CamScrollingNew : MonoBehaviour
{
    [NonSerialized]
    private CinemachineBrain cinemachineBrain;

    [HideInInspector]
    public Action TargetReached;

    private Transform currentTargetTransform;

    public BaseScrollingTarget CurrentBaseScrollingTarget { get; private set; }

    private void Awake()
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }

    private void LateUpdate()
    {
        if (currentTargetTransform == null) return;
        if (transform.position == currentTargetTransform.position)
        {
            TargetReached?.Invoke();
        }
    }

    public void GoToTarget(BaseScrollingTarget target)
    {
        CurrentBaseScrollingTarget = target;
        SetupCameraBlend(target);
        target.EnableVirtualCam(true);
        currentTargetTransform = target.transform;
    }

    private void SetupCameraBlend(BaseScrollingTarget scrollingTarget)
    {
        cinemachineBrain.m_DefaultBlend.m_Style = scrollingTarget.BlendStyle;
        cinemachineBrain.m_DefaultBlend.m_Time = scrollingTarget.CalculatedBlendTime;
    }
}
