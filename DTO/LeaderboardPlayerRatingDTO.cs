using System;
using System.Collections.Generic;
using System.Text;

namespace Logzep.YandexSDK.DTO
{
    [Serializable]
    public class LeaderboardPlayerRatingDTO
    {
        public int Score;
        public int Rank;
        public bool Error;
        public string ErrorMessage;
    }
}
