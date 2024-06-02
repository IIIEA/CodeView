using System;
using System.Collections;
using System.Collections.Generic;
using _Fly_Connect.Scripts.PopupScripts.Window;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.Tutorial.Steps.Welcome
{
    public class TutorialPopup : MonoWindow<ITutorialPopupPresenter>
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _skipButton;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text _title;

        private bool _isTyping;
        private string _targetDescription;
        private string _currentText;
        private RectTransform _rectTransform;
        private ITutorialPopupPresenter _presenter;
        private Coroutine _coroutine;
        private Coroutine _typingDescriptionCoroutine;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        protected override void OnShow(ITutorialPopupPresenter args)
        {
            if (args is not ITutorialPopupPresenter presenter)
            {
                throw new Exception("Expected WelcomePopup Presenter");
            }

            _presenter = presenter;
            _presenter.OnTextUpdate += OnTextUpdate;
            gameObject.SetActive(true);
            _skipButton.gameObject.SetActive(false);
            _text.SetText(_presenter.Text);
            _nextButton.onClick.AddListener(OnNextButtonClicked);
        }

        public void Init(string description, string title, bool isButtonVisible = false)
        {
            _title.text = title;
            _targetDescription = description;
            SwitchButtonState(isButtonVisible);

            if (_coroutine != null)
            {
                _isTyping = false;
                _text.text = "";
                StopCoroutine(_coroutine);
            }

            if (_typingDescriptionCoroutine != null)
                StopCoroutine(_typingDescriptionCoroutine);

            _coroutine = StartCoroutine(TypeTitleRoutine(title));
        }

        private IEnumerator TypeTitleRoutine(string title)
        {
            if (_title.text == title)
            {
                _title.text = "";

                foreach (char letter in title)
                {
                    _title.text += letter;
                    yield return new WaitForSeconds(0.003f);
                }
            }

            _typingDescriptionCoroutine = StartCoroutine(TypeText());
        }

        public void SetHeightWindow(float height)
        {
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, height);
        }

        private void OnTextUpdate()
        {
            _text.SetText(_presenter.Text);
        }

        protected override void OnHide()
        {
            _nextButton.onClick.RemoveListener(OnNextButtonClicked);
        }

        private void OnNextButtonClicked()
        {
            _presenter.OnNextButtonClicked();
            _presenter.OnTextUpdate -= OnTextUpdate;
        }

        private void SwitchButtonState(bool isButtonVisible = false)
        {
            if (_presenter.IsShowButton)
            {
                _nextButton.gameObject.SetActive(!isButtonVisible);
            }
            else
            {
                _nextButton.gameObject.SetActive(isButtonVisible);
            }
        }

        private IEnumerator TypeText()
        {
            _isTyping = true;
            _currentText = "";

            foreach (char letter in _targetDescription)
            {
                _currentText += letter;
                _text.text = _currentText;
                yield return new WaitForSeconds(0.003f);
            }

            _isTyping = false;
        }
    }
}