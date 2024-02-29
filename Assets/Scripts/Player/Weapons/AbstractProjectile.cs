using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public abstract class AbstractProjectile : MonoBehaviour, IPoolable
{
    private const float launchHeight = 5f;

    [SerializeField]
    private FloatStat speed;
    private Rigidbody rb;
    [SerializeField, SuffixLabel("seconds", true)]
    private float duration = 10;
    [SerializeField]
    public bool shootThroughWall = false;

    protected IDamageSource source;
    protected int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(rb.velocity != new Vector3())
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity.normalized);
        }
    }

    public void SetUp(int damage, IDamageSource source)
    {
        this.damage = damage;
        this.source = source;
    }

    public void Shoot(Vector3 direction)
    {
        rb.velocity = direction.normalized * speed;
        transform.LookAt(transform.position + direction);
    }

    public void Launch(Vector3 targetPosition) => StartCoroutine(Launching(targetPosition));
    public void Launch(Transform target) => StartCoroutine(Launching(target));
    protected virtual bool LaunchUpdate(float percent) { return true; }
    protected virtual void LaunchEnd() { }

    protected abstract void Hit(AbstractEnemy enemy);
    public abstract void Destroy();

    private void OnTriggerEnter(Collider other)
    {
        if(shootThroughWall)
        {
            if(other.gameObject.layer != LayerMask.NameToLayer("Obstacle") &&
                other.gameObject.layer != LayerMask.NameToLayer("PlayerOnlyWall"))

            Destroy();
            return;
        }

        var enemy = other.GetComponent<AbstractEnemy>();
        if(enemy != null)
        {
            Hit(enemy);
        }

    }

    private IEnumerator Launching(Transform target)
    {
        Vector3 p0 = transform.position;
        Vector3 p3 = target.transform.position;

        transform.LookAt(p3);

        float time = Vector3.Distance(p3, p0) * (1 / speed);
        float timer = 0;
        float percent = timer / time;

        while (timer < time && LaunchUpdate(percent))
        {
            timer += Time.deltaTime;
            percent = timer / time;

            if (target != null && target.gameObject.activeInHierarchy) p3 = target.transform.position;
            Vector3 p1 = p0 + (p3 - p0) * 0.333f + Vector3.up * launchHeight;
            Vector3 p2 = p0 + (p3 - p0) * 0.666f + Vector3.up * launchHeight;

            transform.position = AdvancedMath.CubeBezier3(p0, p1, p2, p3, timer / time);

            yield return new WaitForEndOfFrame();
        }

        LaunchEnd();
    }

    private IEnumerator Launching(Vector3 targetPosition)
    {
        Vector3 p0 = transform.position;
        Vector3 p3 = targetPosition;

        transform.LookAt(p3);

        float time = Vector3.Distance(p3, p0) * (1 / speed);
        float timer = 0;
        float percent = timer / time;

        while (timer < time && LaunchUpdate(percent))
        {
            timer += Time.deltaTime;
            percent = timer / time;

            Vector3 p1 = p0 + (p3 - p0) * 0.333f + Vector3.up * launchHeight;
            Vector3 p2 = p0 + (p3 - p0) * 0.666f + Vector3.up * launchHeight;

            transform.position = AdvancedMath.CubeBezier3(p0, p1, p2, p3, timer / time);

            yield return new WaitForEndOfFrame();
        }

        LaunchEnd();
    }

    private IEnumerator delayedPooling()
    {
        yield return new WaitForSeconds(duration);
        Destroy();
    }

    #region Ipoolable
    public void OnReturnedToPool()
    {
        StopAllCoroutines();

        InternalOnReturnedToPool();
    }

    public void OnTakenFromPool()
    {
        StartCoroutine(delayedPooling());

        InternalOnTakenFromPool();
    }

    protected virtual void InternalOnReturnedToPool()
    {

    }

    protected virtual void InternalOnTakenFromPool()
    {

    }
    #endregion
}
