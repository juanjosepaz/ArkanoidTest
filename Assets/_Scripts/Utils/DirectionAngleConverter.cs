using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionAngleConverter
{
    public static float GetAngleFromDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return angle;
    }

    public static Vector2 GetDirectionFromAngle(float angle)
    {
        float angleInRadians = angle * Mathf.Deg2Rad;

        Vector2 direction = new(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

        return direction;
    }
}
