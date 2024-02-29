using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class AbstractItem : MonoBehaviour, IPoolable
{
    //collect animation settings
    private const float collectDuration = 0.75f;
    private const float horizontalCollectDistance = 3f;
    private const float verticalCollectionHeight = 1.5f;

    [SerializeField]
    private PoolableSound pickupSound;

    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();    
    }

    protected abstract void PickedUp(Player player);

    private void OnTriggerEnter(Collider other)
    {
        var player = other.transform.GetComponent<Player>();

        if (player != null)
        {
            StartCoroutine(Collect(player));
            if(col != null) col.enabled = false;
        }
    }

    private IEnumerator Collect(Player by)
    {
        var timer = 0f;
        var p0 = transform.position;
        var playedSound = false;

        float percent = 0;

        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / collectDuration;

            var direction = (by.transform.position - p0);

            #region calculate positions
            Vector3 p1 = new Vector3();
            p1 = direction.normalized * horizontalCollectDistance * -1;
            p1.y = 0f;
            p1 += p0;

            Vector3 p2 = new Vector3();
            p2 = direction.normalized * horizontalCollectDistance * -1f;
            p2.y = verticalCollectionHeight * 0.75f;
            p2 += p0;

            Vector3 p3 = new Vector3();
            p3 = by.transform.position;
            p3.y = verticalCollectionHeight;
            #endregion

            CollectUpdate(percent);

            transform.position = AdvancedMath.CubeBezier3(p0, p1, p2, p3, percent);

            if (pickupSound != null && !playedSound && percent > 0.85)
            {
                var sound = Pool.Get<PoolableSound>(pickupSound.name);
                sound.transform.position = transform.position;
                sound.Play();
                playedSound = true;
            }

            yield return new WaitForEndOfFrame();
        }

        PickedUp(by);
    }

    protected virtual void CollectUpdate(float percent)
    {

    }

   

    #region IPoolable
    public void OnReturnedToPool()
    {
        StopAllCoroutines();
        InternalOnReturnedToPool();
        col.enabled = true;
    }

    public void OnTakenFromPool()
    {
        InternalOnTakenfromPool();
    }

    protected virtual void InternalOnReturnedToPool()
    {
        
    }

    protected virtual void InternalOnTakenfromPool()
    {
    }
    #endregion
}
