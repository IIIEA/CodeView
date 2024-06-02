using System;
using System.Collections.Generic;
using Asyncoroutine;
using JetBrains.Annotations;
using MadPixel.InApps;
using MadPixelAnalytics;
using MAXHelper;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Fly_Connect.Scripts.ApplicationLoader.LoadingTasks
{
    [UsedImplicitly]
    public sealed class LoadingTaskInitBanner : ILoadingTask
    {
        public float Weight { get; private set; } = 0.4f;

        public async void Do(Action<LoadingResult> callback)
        {
            LoadingScreen.OnHide += OnLoadingScreenHide;

            AdsManager.Instance.InitApplovin();
            await new WaitUntil(AdsManager.Ready);
            AnalyticsManager.Instance.Init();

            await new WaitForSeconds(0.5f);
            LoadingScreen.ReportProgress(Weight);
            callback.Invoke(LoadingResult.Success());
        }


        private void OnLoadingScreenHide()
        {
            AdsManager.ToggleBanner(true, MaxSdkBase.BannerPosition.BottomCenter);
            LoadingScreen.OnHide -= OnLoadingScreenHide;
        }
    }
}