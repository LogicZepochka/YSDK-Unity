using Logzep.YandexSDK.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Events;

namespace Logzep.YandexSDK.Leaderboards
{
    public class YandexLeaderboards
    {
        public UnityEvent<YandexLeaderboard> OnLeaderboardDescriptionRecieved = new UnityEvent<YandexLeaderboard>();
        public UnityEvent<int, int, LeaderboardError> OnLeaderboardPlayerRatingRecieved = new UnityEvent<int, int, LeaderboardError>();
        public UnityEvent<LeaderboardRating> OnLeaderboardRatingRecieved = new UnityEvent<LeaderboardRating>();


        private readonly YandexSDKComponent _bridgeComponent;

        public YandexLeaderboards(YandexSDKComponent bridgeComponent)
        {
            _bridgeComponent = bridgeComponent;
        }

        public void RequestLeaderboard(string leaderboardKey, UnityAction<YandexLeaderboard> OnDataRecived)
        {
            if (!YandexSDK.IsInitialized)
                throw new Exception("YandexSDK is not initialized!");
            OnLeaderboardDescriptionRecieved.AddListener(OnDataRecived);
            _bridgeComponent.RequestLeaderboard(leaderboardKey);
        }

        public void SetLeaderboardScore(string leaderboardKey, int value)
        {
            if (!YandexSDK.IsInitialized)
                throw new Exception("YandexSDK is not initialized!");
            if (!YandexSDK.GetPlayer.IsAutorized)
                throw new Exception("User authorization in Yandex.Game is required");
            _bridgeComponent.SetPlayerLeaderboardScore(leaderboardKey, value);
        }

        public void GetPlayerLeaderboardRating(string leaderboardKey, UnityAction<int, int, LeaderboardError> OnDataRecived)
        {
            if (!YandexSDK.IsInitialized)
                throw new Exception("YandexSDK is not initialized!");
            if (!YandexSDK.GetPlayer.IsAutorized)
                throw new Exception("User authorization in Yandex.Game is required");
            OnLeaderboardPlayerRatingRecieved.AddListener(OnDataRecived);
            _bridgeComponent.GetLeaderboardPlayerRating(leaderboardKey);
        }

        public void GetLeaderboardRating(string leaderboardKey, UnityAction<LeaderboardRating> OnDataRecived, bool includeUser = false, int quantityAround = 5, int quantityTop = 5)
        {
            if (!YandexSDK.IsInitialized)
                throw new Exception("YandexSDK is not initialized!");
            OnLeaderboardRatingRecieved?.AddListener(OnDataRecived);
            _bridgeComponent.GetLeaderboardRatingList(leaderboardKey,includeUser,quantityAround,quantityTop);
        }
    }
}
