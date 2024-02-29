using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraCage : MonoBehaviour
{
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        var xp = other.GetComponent<XPItem>();
        ReturnXPToPool(xp);
    }

    private void OnTriggerExit(Collider other)
    {
        var xp = other.GetComponent<XPItem>();
        ReturnXPToPool(xp);
    }

    private void ReturnXPToPool(XPItem xp)
    {
        if (xp == null) return;
        if (!xp.IsOnScreen)
        {
            Pool.Return<XPItem>(xp.gameObject);
        }
    }
}
