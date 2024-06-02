using UnityEngine;

namespace _Fly_Connect.Scripts.Sound.UI_Audio_Manager
{
    [AddComponentMenu("Audio/UISound/UI Sound Component")]
    public class UISoundComponent : MonoBehaviour
    {
        public void PlayEnum(UISoundType soundType)
        {
            UISoundManager.PlaySound(soundType);
        }

        public void PlayClip(AudioClip sound)
        {
            UISoundManager.PlaySound(sound);            
        }

        public void Booster()
        {
            UISoundManager.PlaySound(UISoundType.BOOSTER);
        }
    
        public void Click()
        {
            UISoundManager.PlaySound(UISoundType.CLICK);
        }

        public void Error()
        {
            UISoundManager.PlaySound(UISoundType.ERROR);
        }

        public void Accept()
        {
            UISoundManager.PlaySound(UISoundType.ACCEPT);
        }

        public void Close()
        {
            UISoundManager.PlaySound(UISoundType.CLOSE);
        }

        public void Buy()
        {
            UISoundManager.PlaySound(UISoundType.BUY);
        }

        public void AirportBuy()
        {
            UISoundManager.PlaySound(UISoundType.AIRPORT_BUY);
        }

        public void ShowPopup()
        {
            UISoundManager.PlaySound(UISoundType.SHOW_POPUP);
        }
    }
}
