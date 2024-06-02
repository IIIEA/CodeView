using _Fly_Connect.Scripts.Gameplay.BezierCurve;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    public class RotateComponent : MonoBehaviour
    {
        public void Rotate(Vector3 p0, Vector3 p1, Vector3 p2, float t, bool forward)
        {
            Vector3 lookDirection = Bezier.GetFirstDerivative(p0, p1, p2, t).normalized;

            if (!forward)
            {
                lookDirection = -lookDirection;
            }

            Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            if (!forward)
            {
                rotation *= Quaternion.Euler(0, 0, -180);
            }

            transform.rotation = rotation;
        }
    }
}