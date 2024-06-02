using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial.Steps.Welcome
{
    class TutorialPopupPresenter : ITutorialPopupPresenter
    {
        public event Action OnSkipButtonClick;
        public event Action OnNextButtonClick;
        public event Action OnTextUpdate;

        public string Text { get; private set; }
        public bool IsShowButton { get; private set; }

        public void SetText(string text)
        {
            Text = text;
            OnTextUpdate?.Invoke();
        }

        public void OnSkipButtonClicked()
        {
            OnSkipButtonClick?.Invoke();
        }

        public void OnNextButtonClicked()
        {
            OnNextButtonClick?.Invoke();
        }
    }
}