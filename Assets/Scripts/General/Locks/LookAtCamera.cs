using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    private bool lateUpdate;
    [SerializeField]
    private bool fixedUpdate;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update() => Look();

    private void LateUpdate()
    {
        if (lateUpdate)
        {
            Look();
        }
    }

    private void FixedUpdate()
    {
        if (fixedUpdate)
        {
            Look();
        }
    }

    public void Look()
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.back, cam.transform.rotation * Vector3.down);
    }
}
