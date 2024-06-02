using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using Lean.Touch;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CamerasScripts
{
    public class CameraInstaller : GameInstaller
    {
        [SerializeField, Service(typeof(Camera))]
        private Camera _camera;

        [SerializeField, Service(typeof(CameraController))]
        private CameraController _cameraController;

        [SerializeField, Service(typeof(LeanDragCamera))]
        private LeanDragCamera _leanDragCamera;

        [SerializeField, Service(typeof(LeanPinchCamera))]
        private LeanPinchCamera _leanPinchCamera;

        [SerializeField, Service(typeof(BuildingCameraService))]
        private BuildingCameraService _buildingCameraService;

        [SerializeField, Listener, Service(typeof(InputSystem))]
        private InputSystem _inputSystem = new();
    }
}