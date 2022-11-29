using UnityEngine;

public static class YSDKJsonConverter
{

    /// <summary>
    /// Convert JSON to LeaderboardInfo
    /// </summary>
    /// <param name="json"></param>
    /// <returns>Leaderboard description</returns>
    public static LeaderboardDescription ConvertToLeaderboardDescription(string json)
    {
        return JsonUtility.FromJson<LeaderboardDescription>(json);
    }

    /// <summary>
    /// Convert JSON to Leaderboard Data
    /// </summary>
    /// <param name="json"></param>
    /// <returns>Leaderboard rating</returns>
    public static LeaderboardData ConvertToLeaderboardData(string json)
    {
        return JsonUtility.FromJson<LeaderboardData>(json);
    }
}