using TMPro;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.UI
{
    public class TicketWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _moneyText;

        public void SetupTicket(string ticket)
        {
            _moneyText.SetText(ticket);
        }

        public void UpdateTicket(string ticket)
        {
            _moneyText.SetText(ticket);
        }
    }
}