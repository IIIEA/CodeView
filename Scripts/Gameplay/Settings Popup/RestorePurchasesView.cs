using MadPixel.InApps;
using UnityEngine;
using UnityEngine.UI;

public class RestorePurchasesView : MonoBehaviour
{
    [SerializeField] private Button _buyButton;

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
    }

    private void OnBuyButtonClicked()
    {
        MobileInAppPurchaser.Instance.RestorePurchases();
    }
}