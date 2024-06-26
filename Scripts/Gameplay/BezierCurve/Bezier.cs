﻿using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.BezierCurve
{
    public static class Bezier
    {
        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
        
            return oneMinusT * oneMinusT * p0 + 2f * oneMinusT * t * p1 + t * t * p2;
        }

        public static Vector3 GetFirstDerivative(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
        
            return 2f * oneMinusT * (p1 - p0) + 2f * t * (p2 - p1);
        }
    }
}