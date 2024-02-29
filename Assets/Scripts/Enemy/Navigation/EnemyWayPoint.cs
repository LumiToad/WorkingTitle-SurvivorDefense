using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWayPoint : MonoBehaviour
{
    [HideInInspector]
    public EnemyWayPoint nextPoint = null;

    public Line lineToNextPoint
    {
        get
        {
            if (nextPoint == null) return new Line();

            var start = new Vector2(transform.position.x, transform.position.z);
            var end = new Vector2(nextPoint.transform.position.x, nextPoint.transform.position.z);
            return new Line(start, end);
        }
    }
}
