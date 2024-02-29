using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemyNavigation
{
    private List<MoveInformations> path = null;
    private Vector3 magnetTarget = new Vector3();

    private Vector3 movementDirection = new Vector3();

    private float updateSkips = 0;

    public void SetTarget(Vector3 position, int priority)
    {
        magnetTarget = position;
    }

    public void SetUp(Vector3 from)
    {
        path = EnemyPathManager.GetNextPath(from);

        if (path.Count > 0)
        {
            path[0].startedLeft = path.First().toCross.IsLeftFrom(from);
        }
    }

    public Vector3 GetMovementDirection(Vector3 from)
    {
        return movementDirection;
    }

    public void Update(Vector3 from)
    {
        if (updateSkips > 0)
        {
            updateSkips--;
            return;
        }
        updateSkips = 10;
        UpdateMovementDirection(from);

        if (path == null) return;
        if (path.Count == 0) return;

        var info = path.First();
        if (info.startedLeft && info.toCross.IsRightFrom(from) ||
            !info.startedLeft && info.toCross.IsLeftFrom(from))
        {
            path.Remove(path.First());

            if (path.Count > 0)
            {
                path[0].startedLeft = path.First().toCross.IsLeftFrom(from);
            }
        }
    }

    private void UpdateMovementDirection(Vector3 from)
    {
        if (magnetTarget != new Vector3())
        {
            movementDirection = (magnetTarget - from);
            return;
        }
        else if (path.Count > 0)
        {
            var dir = path.First().direction;
            movementDirection = path.First().direction.ToVector3();
            return;
        }

        movementDirection = (new Vector3(movementDirection.x, 0, movementDirection.z)).normalized;
    }
}
