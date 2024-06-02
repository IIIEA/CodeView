using _Fly_Connect.Scripts.Gameplay.AirplaneScripts;
using _Fly_Connect.Scripts.Gameplay.CountryScripts;
using _Fly_Connect.Scripts.Gameplay.Sellers;
using _Fly_Connect.Scripts.Gameplay.Upgrades;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using TMPro;
using UnityEngine;

public class AnalyticPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _cityCountText;
    [SerializeField] private TMP_Text _cityUpgradeCountText;
    [SerializeField] private TMP_Text _routeCountText;
    [SerializeField] private TMP_Text _ufoTakenCount;
    [SerializeField] private TMP_Text _tapUpgradeCount;
    [SerializeField] private TMP_Text _speedUpgradeCount;
    [SerializeField] private TMP_Text _incomeUpgradeCount;

    private Map _map;
    private AirplaneRouteFactory _airplaneRouteFactory;
    private UFOSeller _ufoSeller;
    private UpgradePresenter _upgradePresenter;

    [Inject]
    private void Construct(Map map, AirplaneRouteFactory airplaneRouteFactory, UFOSeller ufoSeller,
        UpgradePresenter upgradePresenter)
    {
        _upgradePresenter = upgradePresenter;
        _ufoSeller = ufoSeller;
        _airplaneRouteFactory = airplaneRouteFactory;
        _map = map;
    }

    private void OnEnable()
    {
        _cityCountText.text = "City Count: " + _map.GetBuyedCities().Count;
        _routeCountText.text = "Route Count: " + _airplaneRouteFactory.Routes.Count;
        _ufoTakenCount.text = "UFO Taken Count: " + _ufoSeller.TakenUFO;
        _tapUpgradeCount.text = "Tap Upgrade Count: " + _upgradePresenter.GetTapIncomeLevel();
        _speedUpgradeCount.text = "Speed Upgrade Count: " + _upgradePresenter.GetSpeedUpgradeLevel();
        _incomeUpgradeCount.text = "Income Upgrade Count: " + _upgradePresenter.GetIncomeUpgradeLevel();

        int[] cityLevelsCount = new int[4]; 

        var cities = _map.GetBuyedCities();

        foreach (var city in cities)
        {
            int cityLevel = city.CurrentAirportLevel;

            if (cityLevel >= 1 && cityLevel <= 4)
            {
                cityLevelsCount[cityLevel - 1]++; 
            }
        }

        _cityUpgradeCountText.text = $"City \n 1 lvl: {cityLevelsCount[0]} \n 2 lvl: {cityLevelsCount[1]} \n 3 lvl: {cityLevelsCount[2]} \n 4 lvl: {cityLevelsCount[3]}";
    }
}