using Logzep.YandexSDK.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logzep.YandexSDK.Leaderboards
{

    public class YandexLeaderboard
    {
        public readonly LeaderboardSortOrder SortOrder;
        public readonly LeaderboardType LeaderboardType;
        public readonly bool Default;
        public readonly Dictionary<string, string> LocalizedName = new Dictionary<string, string>();
        public readonly string Name;

        public YandexLeaderboard(LeaderboardDescriptionDTO DTO)
        {
            Name = DTO.Name;
            Default = DTO.Default;
            SortOrder = (DTO.InvertSortOrder) ? LeaderboardSortOrder.Descending : LeaderboardSortOrder.Ascending;
            LeaderboardType = (DTO.Type == "numberic") ? LeaderboardType.Numeric : LeaderboardType.Time;

            foreach(string map in DTO.LocNameMap)
            {
                if (String.IsNullOrEmpty(map)) continue;
                string[] rawDic = map.Split(':');
                LocalizedName.Add(rawDic[0], rawDic[1]);
            }
        }
    }
}
