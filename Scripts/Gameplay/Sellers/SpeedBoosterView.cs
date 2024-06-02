using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Gameplay.Sellers
{
    public class SpeedBoosterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _clockText;
        [SerializeField] private RectTransform _freePanelTransform;
        [SerializeField] private Image _adIcon;
        [SerializeField] private Sprite _adSprite;
        [SerializeField] private Sprite _ticketSprite;

        [field: SerializeField] public Button Button { get; private set; }

        public void SetAdIcon()
        {
            _adIcon.sprite = _adSprite;
        }

        public void SetTicketIcon()
        {
            _adIcon.sprite = _ticketSprite;
        }

        public void EnableCountDown()
        {
            _clockText.gameObject.SetActive(true);
            _freePanelTransform.gameObject.SetActive(false);
            Button.interactable = false;
        }

        public void DisableCountDown()
        {
            _clockText.gameObject.SetActive(false);
            _freePanelTransform.gameObject.SetActive(true);
            Button.interactable = true;
        }

        public void SetTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

            _clockText.SetText(formattedTime);
        }
    }
}