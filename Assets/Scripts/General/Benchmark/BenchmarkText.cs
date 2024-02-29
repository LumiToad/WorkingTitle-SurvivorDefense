using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BenchmarkText : MonoBehaviour
{
    private const float targetFPS = 60;

    private TextMeshProUGUI text;

    private double fps;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(UpdateFPSLoop());
    }

    private void Update()
    {
        text.text = $"FPS: {fps} \nEnemies: {Pool.GetOutOfPoolCount<AbstractEnemy>()}";
    }

    private IEnumerator UpdateFPSLoop()
    {
        while (true)
        {
            fps = Math.Round(1 / Time.unscaledDeltaTime, 1);

            yield return new WaitForSeconds(0.25f);
        }
    }
}
