using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Dissolve))]
public class EnemyMesh : MonoBehaviour
{
    [SerializeField]
    private float blinkDuration = 0.05f;

    [SerializeField, ColorUsageAttribute(true, true)]
    private Color blinkColor = Color.red;

    public Action disappered;

    private Dictionary<SkinnedMeshRenderer, List<Color>> baseRenderColors = new Dictionary<SkinnedMeshRenderer, List<Color>>();

    private void Awake()
    {
        var renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in renderers)
        {
            var originalColors = new List<Color>();

            foreach (var mat in renderer.materials)
            {
                originalColors.Add(mat.GetColor("_EmissionColor"));
            }

            baseRenderColors.Add(renderer, originalColors);
        }
    }

    public void Reset()
    {
        foreach (var pair in baseRenderColors)
        {
            for (int i = 0; i < pair.Key.materials.Length; i++)
            {
                pair.Key.materials[i].SetColor("_EmissionColor", pair.Value[i]);
            }
        }

        disappered = null;
        GetComponent<Dissolve>().Reset();
    }

    public void Manifest() => GetComponent<Dissolve>().Begin(dissolveMode.manifest);
    public void Disappear() => GetComponent<Dissolve>().Begin(dissolveMode.dissolve);
    public void Blink() => StartCoroutine(blink());

    private void OnEnable() => GetComponent<Dissolve>().dissolved += OnDissolveFinished;
    private void OnDisable() => GetComponent<Dissolve>().dissolved -= OnDissolveFinished;
    private void OnDissolveFinished() => disappered?.Invoke();

    private IEnumerator blink()
    {
        foreach (var pair in baseRenderColors)
        {
            for (int i = 0; i < pair.Key.materials.Length; i++)
            {
                pair.Key.materials[i].SetColor("_BlinkColor", blinkColor);
            }
        }

        yield return new WaitForSeconds(blinkDuration);

        foreach (var pair in baseRenderColors)
        {
            for (int i = 0; i < pair.Key.materials.Length; i++)
            {
                pair.Key.materials[i].SetColor("_BlinkColor", pair.Value[i]);
            }
        }
    }
}
