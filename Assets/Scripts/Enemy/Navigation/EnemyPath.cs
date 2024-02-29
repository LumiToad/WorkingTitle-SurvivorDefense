using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    private List<EnemyWayPoint> wayPoints;

    private Color gizmosColor = Color.red;
    [SerializeField]
    private bool debugAutoEnable = false;

    private void Awake()
    {
        wayPoints = new List<EnemyWayPoint>();
        foreach(var point in GetComponentsInChildren<EnemyWayPoint>())
        {
            wayPoints.Add(point);
        }

        ConnectWayPoints(wayPoints);

        if(debugAutoEnable)
        {
            Activate();
        }
    }

    public void Activate()
    {
        EnemyPathManager.Add(this);
        gizmosColor = Color.green;
    }

    public void DeActivate()
    {
        EnemyPathManager.Remove(this);
        gizmosColor = Color.red;
    }

    public EnemyWayPoint GetClosestWayPointTo(Vector3 position)
    {
        EnemyWayPoint result = null;

        foreach(var point in wayPoints)
        {
            if(result == null || Vector3.Distance(point.transform.position, position) < Vector3.Distance(result.transform.position, position))
            {
                result = point;
            }
        }

        return result;
    }

    private void ConnectWayPoints(List<EnemyWayPoint> points)
    {
        EnemyWayPoint lastPoint = null;
        foreach (var point in points)
        {
            if (lastPoint != null)
            {
                lastPoint.nextPoint = point;
            }

            lastPoint = point;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        var lastPoint = new Vector3();
        foreach(var point in GetComponentsInChildren<EnemyWayPoint>())
        {
            var nextPoint = point.transform.position;

            if(lastPoint != new Vector3())
            {
                Gizmos.DrawLine(lastPoint, nextPoint);
            }

            lastPoint = nextPoint;
        }
    }
}
