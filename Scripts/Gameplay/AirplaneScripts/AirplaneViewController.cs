using System;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Fly_Connect.Scripts.Gameplay.AirplaneScripts
{
    [Serializable]
    public class AirplaneViewController
    {
        [FormerlySerializedAs("_airplanes")] [SerializeField] private GameObject[] _airplanesModel;
        [SerializeField] private GameObject[] _airplanesObject;
        [SerializeField] private GameObject[] _triangleAirplanes;

        private Airplane _airplane;
        public bool IsTriangleRegime { get; set; }

        public void Construct(Airplane airplane)
        {
            _airplane = airplane;
            _airplane.OnLevelUp += OnLevelUp;
        }

        ~AirplaneViewController()
        {
            _airplane.OnLevelUp -= OnLevelUp;
        }

        public void EnableTriangleRegime()
        {
            IsTriangleRegime = true;
    
            _triangleAirplanes.ForEach(airplane => airplane.gameObject.SetActive(false));
            _airplanesObject.ForEach(airplane => airplane.gameObject.SetActive(false));

            var index = _airplane.Level - 1;

            if (index >= 0)
                _triangleAirplanes[index].SetActive(true);
        }

        public void DisableTriangleRegime()
        {
            IsTriangleRegime = false;
   
            _triangleAirplanes.ForEach(airplane => airplane.gameObject.SetActive(false));
            _airplanesObject.ForEach(airplane => airplane.gameObject.SetActive(false));

            var index = _airplane.Level - 1;

            if (index >= 0)
                _airplanesObject[index].SetActive(true);
        }

        private void OnLevelUp(int level)
        {
            _airplanesModel.ForEach(airplane => airplane.gameObject.SetActive(false));

            var index = level - 1;

            if (index >= 0)
                _airplanesModel[index].SetActive(true);
        }
    }
}