using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPosition : MonoBehaviour
{
    private Vector3 pos;

    private void Awake()
    {
        pos = transform.position;
    }

    private void LateUpdate()
    {
        transform.position = pos;
    }
}
