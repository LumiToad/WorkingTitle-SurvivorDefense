using System;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    private CamScrollingNew camScrolling;
    private BaseScrollingTarget target;

    private float scale = 10.0f;
    private float power;
    private float powerOverTime;
    private float duration;
    private Vector3 origin;
    private bool isShaking = false;

    public static CamShake instance;
    public static Action ShakeCompleted;

    private void Awake()
    {
        camScrolling = GetComponent<CamScrollingNew>();
        instance = this;
    }

    public static void Shake(float power, float duration) => instance.StartCamShakeInternal(power, duration);

    public void StartCamShakeInternal(float power, float duration)
    {
        if (isShaking) return;

        target = camScrolling.CurrentBaseScrollingTarget;
        origin = target.gameObject.transform.rotation.eulerAngles;
        this.power = power / scale;
        this.powerOverTime = this.power;
        this.duration = duration;
        isShaking = true;
    }

    private void Update()
    {
        if (powerOverTime > 0)
        {
            if (!isShaking) return;
            float speed = power / duration;
            powerOverTime -= speed * Time.deltaTime;
            CamShakeInternalRotate();
        }
        else
        {
            ResetShakeRotate();
        }
    }

    private void ResetShakeRotate()
    {
        if (target == null) return;

        ShakeCompleted?.Invoke();
        isShaking = false;
        target.gameObject.transform.eulerAngles = origin;
        target = null;
        origin = Vector3.zero;
        powerOverTime = 0;
    }

    private void CamShakeInternalRotate()
    {
        if (target == null) return;

        Vector3 shake = origin + (UnityEngine.Random.onUnitSphere * powerOverTime);
        shake.y /= 2.0f;
        shake.z /= 2.0f;
        target.gameObject.transform.eulerAngles = shake;
    }
}
