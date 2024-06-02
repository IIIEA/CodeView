using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    public class Airplane : MonoBehaviour
    {
        [field: SerializeField] public int Level { get; private set; } = 1;
        [field: SerializeField] public int MaxLevel { get; private set; } = 4;

        [SerializeField] private MoveController _moveController;
        [SerializeField] private AirplaneViewController airplaneViewController;

        private Route _route;

        public event Action<int> OnLevelUp;

        private void Awake()
        {
            _route = GetComponentInParent<Route>();
            airplaneViewController.Construct(this);
            _moveController.Construct(_route.P0, _route.P1, _route.P2, _route);
        }

        public void EnableTriangleRegime()
        {
            if (airplaneViewController.IsTriangleRegime == false)
                airplaneViewController.EnableTriangleRegime();
        }

        public void DisableTriangleRegime()
        {
            if (airplaneViewController.IsTriangleRegime == true)
                airplaneViewController.DisableTriangleRegime();
        }

        public bool HasTriangleRegime => airplaneViewController.IsTriangleRegime;

        public MoveController GetMoveController()
        {
            return _moveController;
        }

        public Route GetRoute()
        {
            return _route;
        }

        public void SetAddedSpeed(float speed)
        {
            _moveController.SetAddedSpeed(speed);
        }

        public void SetLevel(int level)
        {
            Level = level;
            OnLevelUp?.Invoke(Level);
        }

        public bool IsForwardDirection()
        {
            return _moveController.IsForwardDirection();
        }
    }
}