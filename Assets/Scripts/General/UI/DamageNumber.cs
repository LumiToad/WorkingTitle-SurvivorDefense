using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour, IPoolable
{
    private const float variance = 0.25f;

    private TextMeshProUGUI text;
    private Animator anim;

    [SerializeField, HorizontalGroup(0.35f)]
    private float max;
    [SerializeField, HideLabel, HorizontalGroup]
    private Gradient gradient;
    
    public void SetUp(float number)
    {
        transform.position += new Vector3(
            Random.Range(-variance, variance), 
            Random.Range(-variance, variance), 
            Random.Range(-variance, variance)
            );


        anim = GetComponent<Animator>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.text = number.ToString();
        text.color = gradient.Evaluate(number / max);
        anim.Play("Show");
    }

    public void Finished() => Pool.Return<DamageNumber>(this.gameObject);

    #region IPoolable
    public void OnReturnedToPool()
    {
        anim?.StopPlayback();
    }

    public void OnTakenFromPool()
    {
    }
    #endregion
}
