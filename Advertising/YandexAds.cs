using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Events;

namespace Logzep.YandexSDK.Advertising
{
    public class YandexAds
    {
        public UnityEvent<AdvResult> OnAdvertShown = new UnityEvent<AdvResult>();
        public UnityEvent<RewardedAdvResult> OnRewardedAdvertShown = new UnityEvent<RewardedAdvResult>();

        [DllImport("__Internal")]
        private static extern void ShowAd();
        [DllImport("__Internal")]
        private static extern void ShowRewardedAd();

        public YandexAds()
        {

        }

        public void ShowAdv()
        {
            if (!YandexSDK.IsInitialized)
                throw new Exception("YandexSDK is not initialized!");
            ShowAd();
        }

        public void ShowRewardedAdv()
        {
            if (!YandexSDK.IsInitialized)
                throw new Exception("YandexSDK is not initialized!");
            ShowRewardedAd();
        }

        public void ShowAdv(UnityAction<AdvResult> resultCallback)
        {
            ShowAd();
            OnAdvertShown.AddListener(resultCallback);
        }

        public void ShowRewardedAdv(UnityAction<RewardedAdvResult> resultCallback)
        {
            ShowRewardedAd();
            OnRewardedAdvertShown.AddListener(resultCallback);
        }
    }
}
