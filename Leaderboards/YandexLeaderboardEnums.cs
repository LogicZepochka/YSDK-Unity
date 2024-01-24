using System;
using System.Collections.Generic;
using System.Text;

namespace Logzep.YandexSDK.Leaderboards
{
    public enum LeaderboardSortOrder
    {
        Descending,
        Ascending
    }

    public enum LeaderboardError
    {
        None,
        LeaderboardPlayerNotPresent,
        Unknown
    }

    public enum LeaderboardType
    {
        Numeric,
        Time
    }
}
