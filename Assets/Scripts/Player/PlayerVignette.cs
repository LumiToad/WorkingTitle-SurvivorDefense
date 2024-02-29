using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PlayerVignette : MonoBehaviour
{
    [SerializeField, SuffixLabel("seconds", true)]
    private float inDuration = 0.1f;
    [SerializeField, SuffixLabel("seconds", true)]
    private float holdDuration = 0.1f;
    [SerializeField, SuffixLabel("seconds", true)]
    private float holdExtension;
    [SerializeField, SuffixLabel("seconds", true)]
    private float outDuration = 0.1f;

    private float startIntensity;
    private float holdTimer = 0;
    private bool blinking = false;

    private Vignette vignette;

    void Start()
    {
        VolumeProfile volumeProfile = GetComponent<Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(VolumeProfile));

        if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));

        startIntensity = vignette.intensity.value;

    }

    [Button]
    public void Blink()
    {
        if (!blinking)
        {
            StartCoroutine(Blink_());
        }
        else
        {
            holdTimer -= holdExtension;
        }
    }

    private IEnumerator Blink_()
    {
        if (blinking) yield break;
        blinking = true;

        vignette.active = true;
        yield return StartCoroutine(fadeIn());

        holdTimer = 0;
        yield return StartCoroutine(fadeHold());

        yield return StartCoroutine(fadeOut());
        vignette.active = false;

        blinking = false;
    }

    private IEnumerator fadeIn()
    {
        float timer = 0;
        while (timer < inDuration)
        {
            timer += Time.deltaTime;

            vignette.intensity.value = (startIntensity * (timer / inDuration));

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator fadeHold()
    {
        while (holdTimer < holdDuration)
        {
            vignette.intensity.value = startIntensity;
            holdTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator fadeOut()
    {
        float timer = 0;
        while (timer < outDuration)
        {
            if(holdTimer < holdDuration)
            {
                timer = 0;
                yield return StartCoroutine(fadeHold());
            }

            timer += Time.deltaTime;

            vignette.intensity.value = (startIntensity * (1 - (timer / outDuration)));

            yield return new WaitForEndOfFrame();
        }
    }
}
