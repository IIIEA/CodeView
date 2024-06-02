using System.Collections.Generic;
using _Fly_Connect.Scripts.ApplicationLoader;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;
using MadPixelAnalytics;
using UnityEngine;

namespace _Fly_Connect.Scripts.Tutorial
{
    public abstract class TutorialStepController : MonoBehaviour,
        IGameStartListener,
        IGameFinishListener
    {
        [SerializeField] private TutorialStep _step;

        private TutorialManager _tutorialManager;

        [Inject]
        public virtual void Construct(GameContext context)
        {
            _tutorialManager = TutorialManager.Instance;
        }

        public void OnStartGame()
        {
            LoadingScreen.OnHide += OnHide;
        }

        private void OnHide()
        {
            LoadingScreen.OnHide -= OnHide;
            
            _tutorialManager.OnStepFinished += CheckForFinish;
            _tutorialManager.OnNextStep += CheckForStart;

            var stepFinished = _tutorialManager.IsStepPassed(_step);

            if (!stepFinished)
            {
                CheckForStart(_tutorialManager.CurrentStep);
            }
        }

        public void OnFinishGame()
        {
            _tutorialManager.OnStepFinished -= CheckForFinish;
            _tutorialManager.OnNextStep -= CheckForStart;
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnStop()
        {
        }

        protected void NotifyAboutComplete()
        {
            if (_tutorialManager.CurrentStep == _step)
            {
                _tutorialManager.FinishCurrentStep();
            }
        }

        protected void NotifyAboutMoveNext()
        {
            if (_tutorialManager.CurrentStep == _step)
            {
                _tutorialManager.MoveToNextStep();
            }
        }

        protected void NotifyAboutCompleteAndMoveNext()
        {
            if (_tutorialManager.CurrentStep == _step)
            {
                _tutorialManager.FinishCurrentStep();
                _tutorialManager.MoveToNextStep();
            }
        }

        protected bool IsStepFinished()
        {
            return _tutorialManager.IsStepPassed(_step);
        }

        private void CheckForFinish(TutorialStep step)
        {
            if (_step == step)
            {
                OnStop();
            }
        }

        private void CheckForStart(TutorialStep step)
        {
            if (_step == step)
            {
                OnStart();
            }
        }
    }
}