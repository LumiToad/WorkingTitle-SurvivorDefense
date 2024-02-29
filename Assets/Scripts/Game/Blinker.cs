using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer mesh;
    [SerializeField]
    private float blinkDuration = 0.25f;

    float defaultAlpha = 0;

    private void Awake()
    {
        defaultAlpha = mesh.material.GetFloat("_Alpha");
    }

    public void Blink()
    {
        StopAllCoroutines();
        StartCoroutine(blink());
    }

    private IEnumerator blink()
    {
        mesh.material.SetFloat("_Alpha", 1f);

        yield return StartCoroutine(lerpToAlpha(1f, 0.1f));
        yield return StartCoroutine(lerpToAlpha(defaultAlpha, blinkDuration));
    }

    private IEnumerator lerpToAlpha(float alpha, float duration)
    {
        var startValue = mesh.material.GetFloat("_Alpha");

        var timer = 0f;
        while (timer < duration)
        {
            timer += Time.fixedDeltaTime;
            var percent = timer / duration;

            mesh.material.SetFloat("_Alpha", Mathf.Lerp(startValue, alpha, percent));

            yield return new WaitForFixedUpdate();
        }
    }
}
