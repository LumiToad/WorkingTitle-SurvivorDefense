using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationIcon : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private EdgeNotification edge;
    [SerializeField, FoldoutGroup("References")]
    private Canvas world;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateEdgeString(string s) => edge.UpdateText(s);

    private void Update()
    {
        var screenPos = cam.WorldToScreenPoint(transform.position);

        if (screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height)
        {
            edge.Hide();
            world.gameObject.SetActive(true);
        }
        else
        {
            edge.Show();
            world.gameObject.SetActive(false);

            edge.UpdatePosition(screenPos);
        }
    }
}
