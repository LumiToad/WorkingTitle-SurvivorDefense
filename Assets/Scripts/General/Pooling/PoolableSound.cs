using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PoolableSound : MonoBehaviour, IPoolable
{
    public void Play() => StartCoroutine(play());

    private IEnumerator play()
    {
        transform.parent = null;

        var audio = GetComponent<AudioSource>();
        audio.Play();
        while (audio.isPlaying)
        {
            yield return null;
        }

        Pool.Return<PoolableSound>(this.gameObject);
    }

    #region Ipoolable
    public void OnReturnedToPool()
    {
    }

    public void OnTakenFromPool()
    {
        StopAllCoroutines();
    }
    #endregion

}
