using System;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial
{
    [AddComponentMenu("Tutorial/Tutorial Manager")]
    public sealed class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; }

        [SerializeField]
        private TutorialList _stepList;

        private int _currentIndex;
        private bool _isCompleted;

        public bool IsCompleted => _isCompleted;
        public TutorialStep CurrentStep => _stepList[_currentIndex];
        public int CurrentIndex => _currentIndex;

        public event Action<TutorialStep> OnStepFinished;
        public event Action<TutorialStep> OnNextStep;
        public event Action OnCompleted;

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("TutorialManager is already created!");
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void Initialize(bool isCompleted = false, int stepIndex = 0)
        {
            _isCompleted = isCompleted;
            _currentIndex = Mathf.Clamp(stepIndex, 0, _stepList.LastIndex);
        }

        public void FinishCurrentStep()
        {
            if (!_isCompleted)
            {
                OnStepFinished?.Invoke(CurrentStep);
            }
        }

        public void MoveToNextStep()
        {
            if (_isCompleted)
            {
                return;
            }

            if (_stepList.IsLast(_currentIndex))
            {
                _isCompleted = true;
                OnCompleted?.Invoke();
                return;
            }

            _currentIndex++;
            OnNextStep?.Invoke(CurrentStep);
        }

        public bool IsStepPassed(TutorialStep step)
        {
            if (_isCompleted)
            {
                return true;
            }

            return _stepList.IndexOf(step) < _currentIndex;
        }

        public int IndexOfStep(TutorialStep step)
        {
            return _stepList.IndexOf(step);
        }
    }
}