using UnityEngine;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    Image image;

    public static Brightness instance;
    public static float CurrentBrightness {  get; private set; }

    private void Awake()
    {
        image = GetComponent<Image>();
        instance = this;
    }

    private void Start()
    {
        instance.SetBrightnessInternal(1.0f);
        VideoSettingsDTO dto = new();
        SaveFileUtils.TryLoadClassFromJsonFile(ref dto, "videoSettings");

        instance.SetBrightnessInternal(dto.brightness);
    }

    public static void SetBrightness(float value) => instance.SetBrightnessInternal(value);

    private void SetBrightnessInternal(float value)
    {
        value = Mathf.Clamp(value, 0.2f, 1.0f);
        CurrentBrightness = value;
        Color color = Color.black;
        color.a -= value;
        image.color = color;
    }
}
