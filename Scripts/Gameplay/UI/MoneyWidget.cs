using TMPro;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.UI
{
    public class MoneyWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _moneyText;

        public void SetupMoney(string money)
        {
            _moneyText.SetText(money);
        }

        public void UpdateMoney(string money)
        {
            _moneyText.SetText(money);
        }
    }
}