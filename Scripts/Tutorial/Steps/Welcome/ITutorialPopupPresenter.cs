using System;

namespace _Fly_Connect.Scripts.Tutorial.Steps.Welcome
{
    public interface ITutorialPopupPresenter
    {
        void OnSkipButtonClicked();
        void OnNextButtonClicked();
        event Action OnSkipButtonClick;
        event Action OnNextButtonClick;
        string Text { get; }
        bool IsShowButton { get; }
        event Action OnTextUpdate;
    }
}