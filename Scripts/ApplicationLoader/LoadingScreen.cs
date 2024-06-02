using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Fly_Connect.Scripts.ApplicationLoader
{
    public sealed class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Sprite _horizontal;
        [SerializeField] private Sprite _vertical;
        [SerializeField] private Image _image;

        public static LoadingScreen Instance;

        [SerializeField] private TMP_Text _errorText;

        [SerializeField] private LoadingProgressBar _progressBar;

        [SerializeField] private ApplicationLoader _applicationLoader;
        private float _progress;

        public static event Action OnHide;

        private void Awake()
        {
            Instance = this;
            _errorText.text = string.Empty;
            _progressBar.SetProgress(0.0f);
        }

        private void OnEnable()
        {
            _applicationLoader.OnProgressChanged += OnProgressChanged;
        }

        private void OnDisable()
        {
            _applicationLoader.OnProgressChanged -= OnProgressChanged;
        }

        private void Update()
        {
            if (_progress == 1)
                return;

            float aspectRatio = (float) Screen.width / Screen.height;

            if (aspectRatio > 1.0f)
            {
                if (_image.sprite != _horizontal)    
                    _image.sprite = _horizontal;
            }
            else
            {
                if (_image.sprite != _vertical)
                    _image.sprite = _vertical;
            }
        }

        private void OnProgressChanged(float progress)
        {
            ReportProgress(progress);
            _progress = progress;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public static void Show()
        {
            Instance.gameObject.SetActive(true);
        }

        public static void ReportProgress(float progress)
        {
            Instance._progressBar.SetProgress(progress);
        }

        public static void Hide()
        {
            Instance.gameObject.SetActive(false);
            OnHide?.Invoke();
        }

        public static void ReportError(string message)
        {
            Instance._errorText.text = message;
        }
    }
}