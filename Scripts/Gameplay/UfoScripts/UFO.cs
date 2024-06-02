using Dreamteck.Splines;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.UfoScripts
{
    public class UFO : MonoBehaviour
    {
        [field:SerializeField] public SplineFollower SplineFollower { get; private set; }

        public void Construct(SplineComputer spline)
        {
            SplineFollower.spline = spline;

        }
    }
}