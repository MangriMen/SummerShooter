using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Vector2Utils
    {
        public static Vector2 FromAngle(float angle)
        {
            float radAngle = angle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)).normalized;
        }
    }

    public static class TransformUtils
    {
        public static float Angle(Transform pointA, Transform pointB) =>
            Mathf.Atan2(
                pointA.transform.position.y - pointB.transform.position.y,
                pointA.transform.position.x - pointB.transform.position.x
            ) * Mathf.Rad2Deg;

        public static float Angle(Vector2 pointA, Vector2 pointB) =>
            Mathf.Atan2(
                pointA.y - pointB.y,
                pointA.x - pointB.x
            ) * Mathf.Rad2Deg;
    }
    public static class MathExtension
    {
        public static bool IsBetweenRange(this float thisValue, float value1, float value2)
        {
            return thisValue >= Mathf.Min(value1, value2) && thisValue <= Mathf.Max(value1, value2);
        }
    }
}
