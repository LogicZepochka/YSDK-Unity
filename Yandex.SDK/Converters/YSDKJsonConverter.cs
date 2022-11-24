using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class YSDKJsonConverter
{
    public static LeaderboardInfo ConvertToLeaderboardInfo(string json)
    {
        return JsonUtility.FromJson<LeaderboardInfo>(json);
    } 
}