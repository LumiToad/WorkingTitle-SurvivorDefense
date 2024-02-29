using Cinemachine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;

public class BaseScrollingTarget : MonoBehaviour
{
    [HideInInspector, NonSerialized]
    protected CinemachineVirtualCamera virtualCamera;

    [HideInInspector]
    public float CalculatedBlendTime { get; set; }

    [SerializeField, ShowInInspector]
    protected CinemachineBlendDefinition.Style blendStyle = CinemachineBlendDefinition.Style.Linear;

    [HideInInspector]
    public CinemachineBlendDefinition.Style BlendStyle { get => blendStyle; }

    protected virtual void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void EnableVirtualCam(bool value)
    {
        virtualCamera.enabled = value;
    }
}
