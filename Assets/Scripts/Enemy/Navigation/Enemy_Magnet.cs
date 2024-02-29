using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyMagnet : MonoBehaviour
{
    [SerializeField]
    private int priority = 0;
    [SerializeField]
    private bool exit = true;

    private void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        var enemy = other.GetComponent<AbstractEnemy>();
        enemy?.SetTargetPos(transform.position, priority);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!exit) return;
        var enemy = other.GetComponent<AbstractEnemy>();
        enemy?.SetTargetPos(new Vector3(), priority);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
}
