using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRelativePositionToParent : MonoBehaviour
{
    Vector3 delta;

    LookAtCamera look;

    private void Awake()
    {
        delta = transform.position - transform.parent.position;
        look = GetComponent<LookAtCamera>();
    }

    private void Update()
    {
        transform.position = transform.parent.position + delta;
        look?.Look();
    }

    private void LateUpdate()
    {
        transform.position = transform.parent.position + delta;
        look?.Look();
    }
}
