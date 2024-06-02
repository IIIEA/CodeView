using System.Collections;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using UnityEngine;

namespace _Fly_Connect.Scripts.PopupScripts
{
    public sealed class PopupInputController : IGameStartListener, IGameFinishListener
    {
        private PopupManager _popupManager;
        private InputSystem _inputManager;
        private ICoroutineRunner _coroutineRunner;
        private readonly float _delayBetweenUnlockInput = 0.3f;

        [Inject]
        public void Construct(PopupManager popupManager, InputSystem inputManager, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _popupManager = popupManager;
            _inputManager = inputManager;
        }

        public void OnStartGame()
        {
            Subscribe();
        }

        public void OnFinishGame()
        {
            Unsubscribe();
        }

        public void Subscribe()
        {
            _popupManager.OnPopupShown += OnPopupShown;
            _popupManager.OnPopupHidden += OnPopupHidden;
        }

        public void Unsubscribe()
        {
            _popupManager.OnPopupShown -= OnPopupShown;
            _popupManager.OnPopupHidden -= OnPopupHidden;
        }

        private void OnPopupShown(PopupName popupName)
        {
            if (popupName != PopupName.TUTORIAL_POPUP && popupName != PopupName.BUY_CITY_POPUP) 
                _inputManager.SwitchState(InputStateId.LOCK);
        }

        private void OnPopupHidden(PopupName popupName)
        {
            _coroutineRunner.StartCoroutine(UnlockWithDelay());
        }

        private IEnumerator UnlockWithDelay()
        {
            yield return new WaitForSeconds(_delayBetweenUnlockInput);

            if (!_popupManager.HasActivePopups)
            {
                _inputManager.SwitchState(InputStateId.BASE);
            }
        }
    }
}