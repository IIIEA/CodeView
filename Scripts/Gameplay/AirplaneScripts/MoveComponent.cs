using _Fly_Connect.Scripts.Gameplay.BezierCurve;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    public class MoveComponent : MonoBehaviour
    {
        public void Move(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            transform.position = Bezier.GetPoint(p0, p1, p2, t);
        }
    }
}