using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.BezierCurve
{
    [ExecuteAlways]
    public class BezierTest : MonoBehaviour
    {
        public Transform P0;
        public Transform P1;
        public Transform P2;

        [Range(0, 1)]
        public float t;

        void Update()
        {
            transform.position = Bezier.GetPoint(P0.position, P1.position, P2.position, t);
            transform.rotation = Quaternion.LookRotation(Bezier.GetFirstDerivative(P0.position, P1.position, P2.position, t));
        }

        private void OnDrawGizmos()
        {
            int segmentsNumber = 20;
            Vector3 previousPoint = P0.position;

            for (int i = 0; i < segmentsNumber + 1; i++)
            {
                float parameter = (float)i / segmentsNumber;
                Vector3 point = Bezier.GetPoint(P0.position, P1.position, P2.position, parameter);
                Gizmos.DrawLine(previousPoint, point);
                previousPoint = point;
            }
        
        }
    }
}