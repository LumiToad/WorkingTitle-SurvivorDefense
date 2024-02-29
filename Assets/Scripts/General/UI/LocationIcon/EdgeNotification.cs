using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class EdgeNotification : MonoBehaviour
{
    private Rect rect;

    private TextMeshProUGUI text;

    private void Awake()
    {
        rect = GetComponent<RectTransform>().rect;
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateText(string s)
    {
        if(text != null)
        {
            text.text = s;
        }
    }

    public void UpdatePosition(Vector3 screenPos)
    {
        transform.position =
              new Vector3
              (
                  Mathf.Clamp(screenPos.x, rect.width / 2, Screen.width - rect.width / 2),
                  Mathf.Clamp(screenPos.y, rect.height / 2, Screen.height - rect.height / 2),
                  0
              );
    }
}
