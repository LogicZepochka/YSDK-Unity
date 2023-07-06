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
        public static YandexPlayerData GetPlayerData => _playerData;
        public static YandexAdvert Ads => _advertisement;
        public static bool IsInitialized => _bridgeComponent != null;
        public static Environment Env => _enviroment;
        

        public static UnityEvent OnYSDKInit = new UnityEvent();
        public static UnityEvent<YandexPlayer> OnPlayerUpdated = new UnityEvent<YandexPlayer>();
        public static UnityEvent<YandexPlayer> OnPlayerAuth = new UnityEvent<YandexPlayer>();
        public static UnityEvent<RatingResult> OnRatingChanged = new UnityEvent<RatingResult>();

        private static YandexAdvert _advertisement;
        private static YandexPlayer _player;
        private static YandexPlayerData _playerData;
        private static Environment _enviroment;

        private static YandexSDKComponent _bridgeComponent = null;

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
            CheckPlatform();
            _advertisement = new YandexAdvert();
            GameObject go = new GameObject("YandexSDKBridge");
            _bridgeComponent = go.AddComponent<YandexSDKComponent>();
        }

        public static void RateGame()
        {
            CheckInitialized();
            _bridgeComponent.RateGame();
        }

        public static void ApplyEnviroment(string json)
        {
            _enviroment = new Environment(json);
            if(_enviroment == null)
            {
                throw new Exception("failed to load Enviroment!");
            }
            OnYSDKInit?.Invoke();
        }

        public static void RateGame(UnityAction<RatingResult> callback)
        {
            CheckInitialized();
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

        internal static void SavePlayerData(string json)
        {
            CheckInitialized();
        }

        internal static void LoadPlayerData(string[] keys)
        {
            CheckInitialized();
        }
    }
}
