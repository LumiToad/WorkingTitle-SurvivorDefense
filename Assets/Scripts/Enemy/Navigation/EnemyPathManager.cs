using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MoveInformations
{
    public Line toCross;
    public Vector2 direction;

    public bool startedLeft;
}

public static class EnemyPathManager
{
    private static List<EnemyPath> paths = new List<EnemyPath>();

    public static void Add(EnemyPath path) => paths.Add(path);
    public static void Remove(EnemyPath path) => paths.Remove(path);

    private static EnemyWayPoint GetStartingWayPoint(Vector3 from)
    {
        EnemyWayPoint wayPoint = null;

        foreach (var path in paths)
        {
            var point = path.GetClosestWayPointTo(from);

            if (point.nextPoint == null) continue;
            if (wayPoint == null ||
               Vector3.Distance(from, point.transform.position) <
               Vector3.Distance(from, wayPoint.transform.position))
            {
                wayPoint = point;
            }
        }

        return wayPoint;
    }

    public static List<MoveInformations> GetNextPath(Vector3 position)
    {
        var wayPoint = GetStartingWayPoint(position);

        if (wayPoint == null || wayPoint.nextPoint == null) return new List<MoveInformations>();

        var result = new List<MoveInformations>();

        var distance = wayPoint.lineToNextPoint.GetSignedDistance(new Vector2(position.x, position.z));
        while (wayPoint.nextPoint != null && wayPoint.nextPoint.nextPoint != null)
        {
            //get paralel to next point
            var nextParalel = wayPoint.lineToNextPoint.GetParalel(distance);

            //get paralel to next next point
            var nextNextParalel = wayPoint.nextPoint.lineToNextPoint.GetParalel(distance);

            //get point where the paralels intersect 
            var intersection = nextParalel.GetIntersectionWith(nextNextParalel);

            var info = new MoveInformations();
            info.toCross = new Line(intersection, wayPoint.lineToNextPoint.end);
            info.direction = (wayPoint.lineToNextPoint.end - wayPoint.lineToNextPoint.start).normalized;

            Debug.DrawLine(info.toCross.start.ToVector3(), info.toCross.end.ToVector3(), Color.yellow, 1000);

            result.Add(info);

            wayPoint = wayPoint.nextPoint;
        }

        var lastInfo = new MoveInformations();
        lastInfo.toCross = new Line(wayPoint.lineToNextPoint.GetPerpendicular());
        lastInfo.direction = (wayPoint.lineToNextPoint.end - wayPoint.lineToNextPoint.start).normalized;
        Debug.DrawLine(lastInfo.toCross.start.ToVector3(), lastInfo.toCross.end.ToVector3(), Color.yellow, 1000);

        result.Add(lastInfo);

        return result;
      












        /*
        var wayPoint = GetStartingWayPoint(position);
        if (wayPoint == null) return new List<Vector3>();

        float delta = GetDistanceToLine(wayPoint.transform.position, wayPoint.lineToNextPoint + wayPoint.transform.position, position);

        var leftDistance = Vector3.Distance(GetSideVector(delta, wayPoint, false), position);
        var rightDistance = Vector3.Distance(GetSideVector(delta, wayPoint, true), position);
        if(leftDistance > rightDistance)
        {
            delta *= -1;
        }

        Vector3 startPosition = position;

        var result = new List<Vector3>();

        while (wayPoint.nextPoint != null)
        {
            var target = GetNextTargetVector(delta, wayPoint, startPosition);

            var resultPosition = new Vector3();
            resultPosition += startPosition;
            resultPosition += target - wayPoint.nextPoint.transform.position;

            if (target != new Vector3())
            {
                result.Add(target);
            }

            wayPoint = wayPoint.nextPoint;
            startPosition = result.Last();
        }

        return result;
        */
    }

    #region Math Stuff
    /*
        private static Vector3 GetNextTargetVector(float delta, EnemyWayPoint currentWayPoint, Vector3 testPosition)
        {
            (Vector3 start, Vector3 end) v1 = GetWayPointRay(currentWayPoint, delta);
            (Vector3 start, Vector3 end) v2 = GetWayPointRay(currentWayPoint.nextPoint, delta);

            var intersection = GetIntersectionPoint(v1.start, v1.end, v2.start, v2.end);
            if (intersection == new Vector3() && currentWayPoint.nextPoint != null)
            {
                var v3Perpendicular = GetPerpendicular(currentWayPoint.lineToNextPoint.normalized);
                return currentWayPoint.nextPoint.transform.position + v3Perpendicular * delta;
            }

            return intersection;
        }

        private static Vector3 GetSideVector(float delta, EnemyWayPoint point, bool right)
        {
            var direction = point.lineToNextPoint.normalized;
            if (right) direction *= -1;

            var perpendicular = Vector2.Perpendicular(new Vector2(direction.x, direction.z));
            var v3Perpendicular = new Vector3(perpendicular.x, 0, perpendicular.y);

            var result = v3Perpendicular;
            result += point.transform.position;

            return result;
        }

        private static float GetDistanceToLine(Vector3 start, Vector3 end, Vector3 testPosition)
        {
            Vector3 direction = end-start;
            Vector3 startingPoint = start;

            testPosition.y = 0;
            direction.y = 0;
            startingPoint.y = 0;


            Ray ray = new Ray(startingPoint, direction);
            return Vector3.Cross(ray.direction, testPosition - ray.origin).magnitude;
        }

        private static (Vector3 start, Vector3 End) GetWayPointRay(EnemyWayPoint currentWayPoint, float delta)
        {
            const int endless = 1000;

            var v3Perpendicular = GetPerpendicular(currentWayPoint.lineToNextPoint.normalized);

            var startPoint = currentWayPoint.transform.position + v3Perpendicular * delta + currentWayPoint.lineToNextPoint * -endless;

            var endPoint = new Vector3();
            endPoint += currentWayPoint.transform.position;
            endPoint += currentWayPoint.lineToNextPoint * endless;
            endPoint += v3Perpendicular.normalized * delta;

            Debug.DrawLine(startPoint, endPoint, Color.blue, 0.25f);

            return (startPoint, endPoint);
        }

        private static Vector3 GetPerpendicular(Vector3 vector)
        {
            var direction = vector;
            var perpendicular = Vector2.Perpendicular(new Vector2(direction.x, direction.z));
            return new Vector3(perpendicular.x, 0, perpendicular.y);
        }

        #region Thanks Google https://www.habrador.com/tutorials/math/5-line-line-intersection/
        //Line segment-line segment intersection in 2d space by using the dot product
        //p1 and p2 belongs to line 1, and p3 and p4 belongs to line 2 
        private static bool IsIntersecting(Vector3 v1Start, Vector3 v1End, Vector3 v2Start, Vector3 v2End)
        {
            bool isIntersecting = false;

            // Debug.DrawLine(v1Start, v1End, Color.yellow, 1);
            // Debug.DrawLine(v2Start, v2End, Color.blue, 1); 

            if (IsPointsOnDifferentSides(v1Start, v1End, v2Start, v2End) && IsPointsOnDifferentSides(v2Start, v2End, v1Start, v1End))
            {
                isIntersecting = true;
            }

            return isIntersecting;

            static bool IsPointsOnDifferentSides(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
            {
                bool isOnDifferentSides = false;

                //The direction of the line
                Vector3 lineDir = p2 - p1;

                //The normal to a line is just flipping x and z and making z negative
                Vector3 lineNormal = new Vector3(-lineDir.z, lineDir.y, lineDir.x);

                //Now we need to take the dot product between the normal and the points on the other line
                float dot1 = Vector3.Dot(lineNormal, p3 - p1);
                float dot2 = Vector3.Dot(lineNormal, p4 - p1);

                //If you multiply them and get a negative value then p3 and p4 are on different sides of the line
                if (dot1 * dot2 < 0f)
                {
                    isOnDifferentSides = true;
                }

                return isOnDifferentSides;
            }
        }

        //Check if the lines are interesecting in 2d space
        private static Vector3 GetIntersectionPoint(Vector3 l1Start, Vector3 l1End, Vector3 l2Start, Vector3 l2End)
        {
            //3d -> 2d
            Vector2 l1_start = new Vector2(l1Start.x, l1Start.z);
            Vector2 l1_end = new Vector2(l1End.x, l1End.z);

            Vector2 l2_start = new Vector2(l2Start.x, l2Start.z);
            Vector2 l2_end = new Vector2(l2End.x, l2End.z);

            //Direction of the lines
            Vector2 l1_dir = (l1_end - l1_start).normalized;
            Vector2 l2_dir = (l2_end - l2_start).normalized;

            //If we know the direction we can get the normal vector to each line
            Vector2 l1_normal = new Vector2(-l1_dir.y, l1_dir.x);
            Vector2 l2_normal = new Vector2(-l2_dir.y, l2_dir.x);

            //Step 1: Rewrite the lines to a general form: Ax + By = k1 and Cx + Dy = k2
            //The normal vector is the A, B
            float A = l1_normal.x;
            float B = l1_normal.y;

            float C = l2_normal.x;
            float D = l2_normal.y;

            //To get k we just use one point on the line
            float k1 = (A * l1_start.x) + (B * l1_start.y);
            float k2 = (C * l2_start.x) + (D * l2_start.y);


            //Step 2: are the lines parallel? -> no solutions
            if (IsParallel(l1_normal, l2_normal))
            {
                return new Vector3();
            }


            //Step 3: are the lines the same line? -> infinite amount of solutions
            //Pick one point on each line and test if the vector between the points is orthogonal to one of the normals
            if (IsOrthogonal(l1_start - l2_start, l1_normal))
            {
                //Return false anyway
                return new Vector3();
            }

            //Step 4: calculate the intersection point -> one solution
            float x_intersect = (D * k1 - B * k2) / (A * D - B * C);
            float y_intersect = (-C * k1 + A * k2) / (A * D - B * C);

            Vector2 intersectPoint = new Vector2(x_intersect, y_intersect);

            //Step 5: but we have line segments so we have to check if the intersection point is within the segment
            if (IsBetween(l1_start, l1_end, intersectPoint) && IsBetween(l2_start, l2_end, intersectPoint))
            {
                return new Vector3(x_intersect, 0, y_intersect);
            }

            return new Vector3();

            #region internal functions
            //Are 2 vectors orthogonal?
            bool IsOrthogonal(Vector2 v1, Vector2 v2)
            {
                //2 vectors are orthogonal is the dot product is 0
                //We have to check if close to 0 because of floating numbers
                if (Mathf.Abs(Vector2.Dot(v1, v2)) < 0.000001f)
                {
                    return true;
                }

                return false;
            }

            //Are 2 vectors parallel?
            bool IsParallel(Vector2 v1, Vector2 v2)
            {
                //2 vectors are parallel if the angle between the vectors are 0 or 180 degrees
                if (Vector2.Angle(v1, v2) == 0f || Vector2.Angle(v1, v2) == 180f)
                {
                    return true;
                }

                return false;
            }

            //Is a point c between 2 other points a and b?
            bool IsBetween(Vector2 a, Vector2 b, Vector2 c)
            {
                bool isBetween = false;

                //Entire line segment
                Vector2 ab = b - a;
                //The intersection and the first point
                Vector2 ac = c - a;

                //Need to check 2 things: 
                //1. If the vectors are pointing in the same direction = if the dot product is positive
                //2. If the length of the vector between the intersection and the first point is smaller than the entire line
                if (Vector2.Dot(ab, ac) > 0f && ab.sqrMagnitude >= ac.sqrMagnitude)
                {
                    isBetween = true;
                }

                return isBetween;
            }
            #endregion
        }
    */

    #endregion
}
