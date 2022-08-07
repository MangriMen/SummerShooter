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

    public static class AngleUtils
    {
        public static float Angle(Transform from, Transform to) =>
            Mathf.Atan2(
                to.transform.position.y - from.transform.position.y,
                to.transform.position.x - from.transform.position.x
            ) * Mathf.Rad2Deg;

        public static float Angle(Vector2 from, Vector2 to) =>
            Mathf.Atan2(
                to.y - from.y,
                to.x - from.x
            ) * Mathf.Rad2Deg;

        public static float AngleToDirectionAngle(float angle)
        {
            float normalizedAngle = (angle < 0 ? angle + 360 : angle) % 360;

            if (normalizedAngle.IsBetweenRange(46, 135))
            {
                return 90;
            }
            else if (normalizedAngle.IsBetweenRange(136, 225))
            {
                return 180;

            }
            else if (normalizedAngle.IsBetweenRange(226, 315))
            {
                return 270;

            }
            else if (normalizedAngle.IsBetweenRange(316, 0) || normalizedAngle.IsBetweenRange(0, 45))
            {
                return 0;
            }

            return 0;
        }
    }
    public static class MathExtension
    {
        public static bool IsBetweenRange(this float value, float value1, float value2)
        {
            return value >= Mathf.Min(value1, value2) && value <= Mathf.Max(value1, value2);
        }
    }
}
