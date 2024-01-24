using System;
using System.Collections.Generic;
using System.Text;
using static UnityEngine.EventSystems.EventTrigger;

namespace Logzep.YandexSDK.DTO
{
    [Serializable]
    public class LeaderboardRating
    {
        public bool IncludePlayer;
        public int UserRank;
        public List<RatingEntry> Entries;
    }

    [Serializable]
    public class RatingEntry
    {
        public int rank;
        public string playerName;
        public string avatarSrcSm;
        public string avatarSrcMd;
        public string avatarSrcLg;
        public int score;
    }
}
