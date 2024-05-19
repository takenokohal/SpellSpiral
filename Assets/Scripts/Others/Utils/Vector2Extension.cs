using UnityEngine;

namespace Others.Utils
{
    public static class Vector2Extension
    {
        public static Vector2 AngleToVector(float rad)
        {
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }
    }
}