using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMesh : MonoBehaviour
{
    private Color defaultEdgeColor = Color.white;

    [SerializeField]
    private float blinkDuration = 0.05f;

    [SerializeField, ColorUsageAttribute(true, true)]
    private Color blinkColor = Color.red;

    Material mat;

    private void Awake()
    {

        foreach(var m in GetComponent<SkinnedMeshRenderer>().materials)
        {
            if (!m.HasColor("_Edge_Color_1")) continue;

            mat = m;
            defaultEdgeColor = mat.GetColor("_Edge_Color_1");
        }
    }

    public void Blink()
    {
        StopAllCoroutines();
        StartCoroutine(blink());
    }

    private IEnumerator blink()
    {
        mat.SetFloat("_EdgeWith", 10);

        mat.SetColor("_Edge_Color_1", blinkColor);

        yield return new WaitForSeconds(blinkDuration);

        mat.SetColor("_Edge_Color_1", defaultEdgeColor);

        mat.SetFloat("_EdgeWith", 0);


    }
}
