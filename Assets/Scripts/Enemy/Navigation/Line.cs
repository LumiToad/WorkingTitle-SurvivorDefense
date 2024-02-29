using UnityEngine;

public class Line 
{
    public Vector2 start;
    public Vector2 end;

    public Line()
    {
        start = new Vector2();
        end = new Vector2();
    }

    public Line(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end = end;
    }

    public Line(Vector3 start, Vector3 end)
    {
        this.start = start.ToVector2();
        this.end = end.ToVector2();
    }

    public Line(Line other)
    {
        this.start = other.start;
        this.end = other.end;
    }

    public Line GetParalel(float distance)
    {
        var perpendicular = Vector2.Perpendicular(end - start);
        var paralel = new Line(this) + perpendicular.normalized * distance;
        return paralel;
    }

    public Vector2 GetIntersectionWith(Line other)
    {
        var A1 = start;
        var A2 = end;
        var B1 = other.start;
        var B2 = other.end;

        float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);

        if (tmp == 0)
        {
            // No solution!
            return Vector2.zero;
        }

        float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;

        var result = new Vector2(
            B1.x + (B2.x - B1.x) * mu,
            B1.y + (B2.y - B1.y) * mu
        );

        return result;
    }

    public float GetSignedDistance(Vector2 point)
    {
        var p3 = point.ToVector3();
        var s3 = start.ToVector3();
        var e3 = end.ToVector3();

        var distance = Vector3.Magnitude(ProjectPointLine(p3, s3, e3) - p3);

        if (IsLeftFrom(point))
        {
            distance *= -1;
        }

        return distance;

        Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            Vector3 rhs = point - lineStart;
            Vector3 vector = lineEnd - lineStart;
            float magnitude = vector.magnitude;
            Vector3 vector2 = vector;
            if (magnitude > 1E-06f)
            {
                vector2 /= magnitude;
            }

            float value = Vector3.Dot(vector2, rhs);
            value = Mathf.Clamp(value, 0f, magnitude);
            return lineStart + vector2 * value;
        }
    }

    public float GetSignedDistance(Vector3 point) => GetSignedDistance(point.ToVector2());

    public bool IsLeftFrom(Vector2 point)
    {
        var A = end - start;
        var B = start - point;

        if (-A.x * B.y + A.y * B.x < 0)
        {
            return true;
        }

        return false;
    }

    public bool IsLeftFrom(Vector3 point) => IsLeftFrom(point.ToVector2());
    public bool IsRightFrom(Vector3 point) => !IsLeftFrom(point.ToVector2());
    public bool IsRightFrom(Vector2 point) => !IsLeftFrom(point);

    public Line GetPerpendicular()
    {
        var dir = Vector2.Perpendicular(end - start).normalized;
        return new Line(end, end + dir);
    }

    #region OperatorOverloadings
    public static Line operator +(Line a, Vector2 b)
    {
        a.start += b;
        a.end += b;
        return a;
    }

    public static Line operator -(Line a, Vector2 b)
    {
        a.start -= b;
        a.end -= b;
        return a;
    }

    public static Line operator *(Line a, Vector2 b)
    {
        a.start *= b;
        a.end *= b;
        return a;
    }

    public static Line operator /(Line a, Vector2 b)
    {
        a.start /= b;
        a.end /= b;
        return a;
    }
    #endregion
}
