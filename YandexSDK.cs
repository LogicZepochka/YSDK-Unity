using Logzep.YandexSDK.Advertising;
using Logzep.YandexSDK.DTO;
using Logzep.YandexSDK.Player;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Logzep.YandexSDK
{
    public static class YandexSDK
    {
        public static string Version => "2.0.2";

        public static YandexPlayer GetPlayer => _player;
        public static YandexAdvert Ads => _advertisement;

        private static YandexPlayer _player;

        private static YandexSDKComponent _bridgeComponent = null;

        public static UnityEvent OnYSDKInit = new UnityEvent();
        public static UnityEvent<YandexPlayer> OnPlayerUpdated = new UnityEvent<YandexPlayer>();
        public static UnityEvent<YandexPlayer> OnPlayerAuth = new UnityEvent<YandexPlayer>();
        public static UnityEvent<RatingResult> OnRatingChanged = new UnityEvent<RatingResult>();

        private static YandexAdvert _advertisement;

        private static void CheckPlatform()
        {
            if(Application.platform != RuntimePlatform.WebGLPlayer)
            {
                throw new Exception("YSDK-Unity can be launched only on WebGL platform");
            }
        }

        public static void Init()
        {
            if(_bridgeComponent != null && _bridgeComponent.gameObject != null)
            {
                Debug.LogWarning("YandexSDK-Unity alredy initialized");
                return;
            }
            _advertisement = new YandexAdvert();
            CheckPlatform();
            GameObject go = new GameObject("YandexSDKBridge");
            _bridgeComponent = go.AddComponent<YandexSDKComponent>();
        }

        public static void RateGame()
        {
            CheckInitialized();
            _bridgeComponent.RateGame();
        }

        public static void RateGame(UnityAction<RatingResult> callback)
        {
            OnRatingChanged.AddListener(callback);
            RateGame();
        }

        public static YandexPlayer ParsePlayerInfo(string json)
        {
            CheckInitialized();
            PlayerDTO playerDTO = JsonUtility.FromJson<PlayerDTO>(json);
            if (playerDTO == null)
            {
                throw new Exception("Failed to parse YaPlayer json");
            }
            _player = new YandexPlayer(playerDTO);
            OnPlayerUpdated?.Invoke(_player);
            return _player;
        }

        private static void CheckInitialized()
        {
            if (_bridgeComponent == null)
                throw new Exception("YandexSDK-Unity bridge is not initialized!");
        }


    }
}
