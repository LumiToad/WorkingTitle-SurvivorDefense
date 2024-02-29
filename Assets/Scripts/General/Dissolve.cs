using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum dissolveMode
{
    dissolve,
    manifest
}

public class Dissolve : MonoBehaviour
{
    public Action dissolved;
    public Action manifested;

    [SerializeField, MinValue(0), SuffixLabel("seconds", true)]
    private float manifestDuration = 0.5f;
    [SerializeField, MinValue(0), SuffixLabel("seconds", true)]
    private float dissolveDuration = 0.5f;
    [SerializeField]
    private bool startInvisible;

    private Dictionary<Material, float> startingCutoffHeight = new Dictionary<Material, float>();

    private void Awake()
    {
        startingCutoffHeight = new Dictionary<Material, float>();
        foreach(var mat in GetMats())
        {
            if (!startingCutoffHeight.ContainsKey(mat))
            {
                startingCutoffHeight.Add(mat, mat.GetFloat("_Cutoff_Height"));
            }

            if (startInvisible) mat.SetFloat("_Cutoff_Height", -1);
        }
    }

    public void Begin(dissolveMode mode) => StartCoroutine(CallOnAllMaterials(mode));

    public void Reset()
    {
        StopAllCoroutines();
        foreach(var mat in GetMats())
        {
            if (!startingCutoffHeight.Keys.Contains(mat)) continue;

            mat.SetFloat("_Cutoff_Height", startingCutoffHeight[mat]);
        }

        dissolved = null;
        manifested = null;
    }

    private IEnumerator CallOnAllMaterials(dissolveMode mode)
    {
        List<Coroutine> coroutines = new List<Coroutine>();
        foreach(var mat in GetMats())
        {
            switch (mode)
            {
                case dissolveMode.dissolve:
                    coroutines.Add(StartCoroutine(DissolveOverTime(mat)));
                    break;
                case dissolveMode.manifest:
                    coroutines.Add(StartCoroutine(ManifestOverTime(mat)));
                    break;
            }
        }

        foreach (var c in coroutines)
        {
            yield return c;
        }

        switch (mode)
        {
            case dissolveMode.dissolve:
                dissolved?.Invoke();
                break;
            case dissolveMode.manifest:
                manifested?.Invoke();
                break;
        }
    }

    private IEnumerator DissolveOverTime(Material mat)
    {
        if (!Application.isPlaying) yield break;

        yield return new WaitForEndOfFrame();

        float timer = 0;
        var full = startingCutoffHeight[mat];

        while (timer < dissolveDuration / 2)
        {
            timer += Time.deltaTime;

            mat.SetFloat("_Cutoff_Height", full * (1 - (timer / (dissolveDuration / 2))));

            yield return new WaitForEndOfFrame();
        }

        timer = 0;
        while (timer < dissolveDuration / 2)
        {
            timer += Time.deltaTime;

            mat.SetFloat("_Cutoff_Height",  -2 * (timer / (dissolveDuration / 2)));

            yield return new WaitForEndOfFrame();
        }

        mat.SetFloat("_Cutoff_Height", -2);
    }

    private IEnumerator ManifestOverTime(Material mat)
    {
        yield return new WaitForEndOfFrame();

        float timer = 0;
        var full = startingCutoffHeight[mat];

        mat.SetFloat("_Cutoff_Height", 0);

        while (timer < manifestDuration)
        {
            timer += Time.deltaTime;

            mat.SetFloat("_Cutoff_Height", full * (timer / manifestDuration));

            yield return new WaitForEndOfFrame();
        }

        mat.SetFloat("_Cutoff_Height", full);
    }

    private List<Material> GetMats()
    {
        if(!Application.isPlaying) return new List<Material>();

        var result = new List<Material>();

        foreach(var renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach(Material mat in renderer.materials)
            {
                if (!mat.HasFloat("_Cutoff_Height")) continue;

                result.Add(mat);
            }
        }

        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            foreach (Material mat in renderer.materials)
            {
                if (!mat.HasFloat("_Cutoff_Height")) continue;

                result.Add(mat);
            }
        }

        return result;
    }
}
