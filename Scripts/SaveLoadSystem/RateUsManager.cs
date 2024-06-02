using System;
using _Fly_Connect.Scripts.Infrastructure.Attributes;
using _Fly_Connect.Scripts.Infrastructure.GameManager;

// #if UNITY_ANDROID
// using Google.Play.Review;
// #endif

#if UNITY_IOS
using UnityEngine;
using UnityEngine.iOS;
// using UnityEngine.WSA;
#endif

namespace _Fly_Connect.Scripts.SaveLoadSystem
{
    public sealed class RateUsManager : IGameStartListener
    {
// #if UNITY_ANDROID
//         private ReviewManager _reviewManager;
//         private PlayReviewInfo _playReviewInfo;
// #endif
        private ICoroutineRunner _coroutineRunner;

        public bool IsRated { get; private set; }

        public event Action OnRateUs;
        public event Action OnErrorRateUs;

        [Inject]
        private void Construct(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Setup(bool isRated)
        {
            IsRated = isRated;
        }

        public void OnStartGame()
        {
// #if UNITY_ANDROID
//             _reviewManager = new ReviewManager();
// #endif
        }

        public void RateUs()
        {
// #if UNITY_ANDROID
//             _coroutineRunner.StartCoroutine(ShowRateUs());
// #endif
#if UNITY_IOS
            Application.OpenURL("https://apps.apple.com/de/app/fly-connect-explore-the-world/id6479404655");
            // Device.RequestStoreReview();
#endif
        }
//
// #if UNITY_ANDROID
//         private IEnumerator ShowRateUs()
//         {
//             var requestFlowOperation = _reviewManager.RequestReviewFlow();
//
//             yield return requestFlowOperation;
//
//             if (requestFlowOperation.Error != ReviewErrorCode.NoError)
//             {
//                 IsRated = false;
//                 OnErrorRateUs?.Invoke();
//                 yield break;
//             }
//
//             _playReviewInfo = requestFlowOperation.GetResult();
//
//             _coroutineRunner.StartCoroutine(PlayReviewFlowRoutine());
//         }
// #endif
//
// #if UNITY_ANDROID
//         private IEnumerator PlayReviewFlowRoutine()
//         {
//             var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
//             yield return launchFlowOperation;
//             _playReviewInfo = null;
//
//             if (launchFlowOperation.Error != ReviewErrorCode.NoError)
//             {
//                 OnErrorRateUs?.Invoke();
//                 IsRated = false;
//                 yield break;
//             }
//
//             IsRated = true;
//             OnRateUs?.Invoke();
//         }
// #endif
     }     
}