using System;
using System.Collections.Generic;
using _Fly_Connect.Scripts;
using _Fly_Connect.Scripts.Gameplay;
using _Fly_Connect.Scripts.Gameplay.CityScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using _Fly_Connect.Scripts.PopupScripts;
using _Fly_Connect.Scripts.Sound.Scene_Audio_Manager;
using _Fly_Connect.Scripts.Sound.UI_Audio_Manager;
using _Fly_Connect.Scripts.Tutorial;
using DG.Tweening;
using Lean.Touch;
using Sirenix.Utilities;
using UnityEngine;

[Serializable]
public class CityShower : IGameStartListener, IGameFinishListener
{
    [SerializeField] private ParticleContainer _particle;

    private Map _map;
    private LeanPinchCamera _leanPinchCamera;
    private List<CityPoint> _cities = new();
    private CameraController _cameraController;
    private LeanDragCamera _leanDragCamera;
    private PopupManager _popupManager;
    private float _targetZoom = 1.4f;
    private Sequence _punchTween;
    private float _zoomDuration = 1f;
    public bool IsPlayAnimation { get; private set; }

    [Inject]
    private void Construct(LeanPinchCamera leanPinchCamera, CameraController cameraController, Map map,
        LeanDragCamera leanDragCamera, PopupManager popupManager)
    {
        _popupManager = popupManager;
        _leanDragCamera = leanDragCamera;
        _cameraController = cameraController;
        _leanPinchCamera = leanPinchCamera;
        _map = map;
    }

    public void OnStartGame()
    {
        _cities = _map.GetGameCities();
        _cities.ForEach(city => city.OnCityUpgrade += OnCityUpgrade);
    }

    public void OnFinishGame()
    {
        _cities.ForEach(city => city.OnCityUpgrade -= OnCityUpgrade);
    }

    private void OnCityUpgrade(CityPoint cityPoint)
    {
        if (!TutorialManager.Instance.IsCompleted)
            return;

        var hasLayer = cityPoint.CityUpLevelController.HasLayerWithoutCityLayer();

        if (cityPoint.CurrentAirportLevel == 4 && !hasLayer)
        {
            _leanDragCamera.enabled = false;
            _popupManager.HidePopup(PopupName.CITY_POPUP);

            if (cityPoint.ShowplaceTransform != null)
            {
                IsPlayAnimation = true;
                var startScale = new Vector3(cityPoint.ShowplaceTransform.transform.localScale.x,
                    cityPoint.ShowplaceTransform.transform.localScale.y,
                    cityPoint.ShowplaceTransform.transform.localScale.z) / 2;

                cityPoint.ShowplaceTransform.DOScale(startScale, 0.01f);

                _cameraController.MoveWithZoomTo(cityPoint.ShowplaceTransform, _zoomDuration, _targetZoom, (() =>
                {
                    _particle.transform.position = cityPoint.ShowplaceTransform.transform.position;
                    _particle.ParticleSystems.ForEach(particle => particle.Play());
                    _leanDragCamera.enabled = true;
                    _leanPinchCamera.enabled = true;
                    UISoundManager.PlaySound(UISoundType.AIRPORT_MAX_LEVEL);
                    StartScaleAnimation(cityPoint, startScale);
                }));
            }
        }
    }


    private void StartScaleAnimation(CityPoint cityPoint, Vector3 startScale)
    {
        _punchTween = DOTween.Sequence();

        _punchTween
            .Append(cityPoint.ShowplaceTransform.DOScale(startScale * 2.4f, 0.3f))
            .Append(cityPoint.ShowplaceTransform.DOScale(startScale * 2, 0.3f))
            .OnComplete((() => IsPlayAnimation = false)).OnComplete((() => IsPlayAnimation = false));
    }
}