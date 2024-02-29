using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceVFX : MonoBehaviour
{
    private const float height = 5f;
    private const float lean = 0f;

    public void Place(float duration, Vector3 target) => StartCoroutine(place(duration, target));

    private IEnumerator place(float duration, Vector3 target)
    {
        GetComponentInChildren<TrailRenderer>().time = duration * 0.5f;

        var p0 = transform.position;
        var p3 = target + new Vector3(0,1,0);

        var leanDirection = Random.Range(0f,1f) > 0.5f ? Vector3.left : Vector3.right;
        leanDirection *= Random.Range(lean / 2, lean * 2);

        var direction = p3 - p0;

        var p1 = p0 + (p3 - p0) * 0.66f + Vector3.up * height + leanDirection;
        var p2 = p0 + (p3 - p0) * 0.33f + Vector3.up * height;

        var timer = duration;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position = AdvancedMath.CubeBezier3(p0, p1, p2, p3, 1 - (timer / duration));
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
