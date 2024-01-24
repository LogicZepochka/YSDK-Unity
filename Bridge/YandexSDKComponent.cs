using Logzep.YandexSDK.Advertising;
using Logzep.YandexSDK.DTO;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using Logzep.YandexSDK.Leaderboards;

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
        [DllImport("__Internal")]
        private static extern void GetEnviroment();
        [DllImport("__Internal")]
        private static extern void RequestLeaderboardDescription(string lbName);
        [DllImport("__Internal")]
        private static extern void SetLeaderboardScore(string lbName,int value);
        [DllImport("__Internal")]
        private static extern void GetLeaderboardPRating(string lbName);
        [DllImport("__Internal")]
        private static extern void GetLeaderboardRating(string lbName,int aroundPlayer,int top,bool includePlayer);

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

        public void GetLeaderboardRatingList(string leaderboardKey, bool includeUser, int quantityAround, int quantityTop)
        {
            GetLeaderboardRating(leaderboardKey, quantityAround, quantityTop, includeUser);
        }

        public void RequestLeaderboard(string leaderboardKey)
        {
            RequestLeaderboardDescription(leaderboardKey);
        }

        public void SetPlayerLeaderboardScore(string leaderboardKey,int value)
        {
            SetLeaderboardScore(leaderboardKey, value);
        }

        public void GetLeaderboardPlayerRating(string leaderboardKey) 
        {
            GetLeaderboardPRating(leaderboardKey);
        }

        public void YSDK_InitPlayer(string playerJson)
        {
            YandexSDK.ParsePlayerInfo(playerJson);
            GetEnviroment();
        }

        public void YSDK_InitEnviroment(string json)
        {
            YandexSDK.ApplyEnviroment(json);
            InitFinish(Application.productName, Application.version, YandexSDK.Version);
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
        }

        public void YSDK_LeaderboardDescriptionRecived(string jsonLeaderboardDescription)
        {
            LeaderboardDescriptionDTO lbDTO = JsonUtility.FromJson<LeaderboardDescriptionDTO>(jsonLeaderboardDescription);
            YandexSDK.Leaderboards.OnLeaderboardDescriptionRecieved?.Invoke(new YandexLeaderboard(lbDTO));
            YandexSDK.Leaderboards.OnLeaderboardDescriptionRecieved?.RemoveAllListeners();
        }

        public void YSDK_RewardedAdResult(int result)
        {
            RewardedAdvResult AdReslt = (RewardedAdvResult)result;
            YandexSDK.Ads.OnRewardedAdvertShown?.Invoke(AdReslt);
        }

        public void YSDK_PlayerDataRecieved(string[] keys,string[] values)
        {
            if (keys.Length != values.Length)
                throw new Exception("Recieved strange data from YandexGames (keys length no equal values length)");
            YandexSDK.GetPlayerData.SaveData(keys, values);
        }

        public void YSDK_LeaderboardPlayerRatingRecived(string jsonLeaderboardPlayerRating)
        {
            LeaderboardPlayerRatingDTO lbPRDTO = JsonUtility.FromJson<LeaderboardPlayerRatingDTO>(jsonLeaderboardPlayerRating);
            LeaderboardError errorCode;
            if(lbPRDTO.Error)
            {
                if (lbPRDTO.ErrorMessage == "LEADERBOARD_PLAYER_NOT_PRESENT")
                {
                    errorCode = LeaderboardError.LeaderboardPlayerNotPresent;
                }
                else
                {
                    errorCode = LeaderboardError.Unknown;
                }
            }
            else
                errorCode = LeaderboardError.None;

            YandexSDK.Leaderboards.OnLeaderboardPlayerRatingRecieved?.Invoke(lbPRDTO.Rank, lbPRDTO.Score, errorCode);
            YandexSDK.Leaderboards.OnLeaderboardPlayerRatingRecieved?.RemoveAllListeners();
        }

        public void YSDK_LeaderboardRatingRecived(string jsonLeaderboardRating)
        {
            LeaderboardRating Rating = JsonUtility.FromJson<LeaderboardRating>(jsonLeaderboardRating);
            YandexSDK.Leaderboards.OnLeaderboardRatingRecieved?.Invoke(Rating);
            YandexSDK.Leaderboards.OnLeaderboardRatingRecieved?.RemoveAllListeners();
        }
    }
}
