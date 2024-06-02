using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.UfoScripts
{
    public class UFOPathGenerator
    {
        private int _numberOfPoints = 10;
        private float _verticalOffset = 2f;

        public Vector3[] GeneratePoints()
        {
            Vector3 leftPoint = Camera.main.ViewportToWorldPoint(new Vector3(-0.5f, Random.Range(0.2f, 0.8f), 10));
            Vector3 rightPoint = Camera.main.ViewportToWorldPoint(new Vector3(1.5f, Random.Range(0.2f, 0.8f), 10));

            Vector3[] points = new Vector3[_numberOfPoints + 2];

            points[0] = leftPoint;
            points[_numberOfPoints + 1] = rightPoint;

            for (int i = 1; i <= _numberOfPoints; i++)
            {
                float t = i / (float)(_numberOfPoints + 1);
                points[i] = Vector3.Lerp(leftPoint, rightPoint, t);
                points[i].y += Random.Range(-_verticalOffset, _verticalOffset);
            }

            return points;
        }
    }
}