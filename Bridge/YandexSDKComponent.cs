using Logzep.YandexSDK.Advertising;
using Logzep.YandexSDK.DTO;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Logzep.YandexSDK
{
    public class YandexSDKComponent: MonoBehaviour
    {
        public static YandexSDKComponent Instance;

        [DllImport("__Internal")]
        private static extern void InitFinish(string gameName, string gameVersion, string YSDKVersion);
        [DllImport("__Internal")]
        private static extern void InitStart();
        [DllImport("__Internal")]
        private static extern void Rate();

        public void Start()
        {
            if(Instance != null)
            {
                Debug.LogWarning("Объект с YandexSDK уже создан.");
                Destroy(gameObject);
            }
            Instance = this;
            gameObject.name = "YandexSDKBridge";
            DontDestroyOnLoad(gameObject);
            Debug.Log("YSDK-Unity: Created yandex-unity bridge object");
            InitStart();
        }

        public void RateGame()
        {
            Rate();
        }


        public void YSDK_InitPlayer(string playerJson)
        {
            YandexSDK.ParsePlayerInfo(playerJson);
            InitFinish(Application.productName, Application.version, YandexSDK.Version);
            YandexSDK.OnYSDKInit?.Invoke();
        }

        public void YSDK_RateResult(int result)
        {
            RatingResult RatingResult = (RatingResult)result;
            YandexSDK.OnRatingChanged?.Invoke(RatingResult);
            YandexSDK.OnRatingChanged?.RemoveAllListeners();
        }

        public void YSDK_AdResult(int result)
        {
            AdvResult AdReslt = (AdvResult)result;
            YandexSDK.Ads.OnAdvertShown?.Invoke(AdReslt);
            YandexSDK.OnRatingChanged?.RemoveAllListeners();
        }

        public void YSDK_RewardedAdResult(int result)
        {
            RewardedAdvResult AdReslt = (RewardedAdvResult)result;
            YandexSDK.Ads.OnRewardedAdvertShown?.Invoke(AdReslt);
            YandexSDK.OnRatingChanged?.RemoveAllListeners();
        }
    }
}
