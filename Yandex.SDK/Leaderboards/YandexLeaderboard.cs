using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class YandexLeaderboard : MonoBehaviour
{


    public UnityEvent<LeaderboardDescription> OnLeaderboardDescriptionReceived;
    public UnityEvent<bool> OnLeaderboardAvailableReceived;
    public UnityEvent<LeaderboardRatingStatus, LeaderboardEntry> OnLeaderboardRatingReceived;
    public UnityEvent<LeaderboardData> OnLeaderboardDataReceived;

    [DllImport("__Internal")]
    private static extern void AskLeaderboardDescription(string name);

    [DllImport("__Internal")]
    private static extern void AskLeaderboardAvailable();
    [DllImport("__Internal")]
    private static extern void AskSetLeaderboardScore(string name,int value);
    [DllImport("__Internal")]
    private static extern void AskPlayerLeaderboardRating(string name);

    [DllImport("__Internal")]
    private static extern void RequestLeaderboard(string name, bool includeUser, int quantityAround, int quantityTop);

    public void GetLeaderboardDescription(string name, UnityAction<LeaderboardDescription> callback)
    {
        Debug.Log("YSDK: Asking LD Description");
        OnLeaderboardDescriptionReceived.AddListener(callback);
        AskLeaderboardDescription(name);
    }

    public void IsLeaderboardAddScoreAvailable(UnityAction<bool> callback)
    {
        Debug.Log("YSDK: Asking LD available");
        OnLeaderboardAvailableReceived?.AddListener(callback);
        AskLeaderboardAvailable();
    }

    public void YSCB_ReceiveLeaderboardDescription(string desc)
    {
        Debug.Log("YSDK: Received LD Description");
        LeaderboardDescription info = YSDKJsonConverter.ConvertToLeaderboardDescription(desc);
        OnLeaderboardDescriptionReceived?.Invoke(info);
        OnLeaderboardDescriptionReceived?.RemoveAllListeners();
    }

    public void YSCB_ReceiveLeaderboardAvailable(bool available)
    {
        Debug.Log("YSDK: Received LD Available");
        OnLeaderboardAvailableReceived?.Invoke(available);
        OnLeaderboardAvailableReceived?.RemoveAllListeners();
    }

    public void SetLeaderboardScore(string name,int value)
    {
        AskSetLeaderboardScore(name, value);
    }

    public void GetLeaderboardRating(string lbName,UnityAction<LeaderboardRatingStatus, LeaderboardEntry> callback)
    {
        OnLeaderboardRatingReceived.AddListener(callback);
        AskPlayerLeaderboardRating(lbName);
    }

    public void YSCB_LeaderboardRatingCallback(string json)
    {
        Debug.Log("LeaderboardRatingCallback called!");
        if (json == "LEADERBOARD_PLAYER_NOT_PRESENT")
        {
            Debug.Log("Error code: LEADERBOARD_PLAYER_NOT_PRESENT");
            OnLeaderboardRatingReceived?.Invoke(LeaderboardRatingStatus.PlayerNotPresent, null);
            return;
        }
        Debug.Log("Parsing LB Rating...");
        LeaderboardEntry leaderboardInfo = JsonUtility.FromJson<LeaderboardEntry>(json);
        Debug.Log($"Parsed! Rank: {leaderboardInfo.score}, Score: {leaderboardInfo.score}");
        Debug.Log("Sending to caller...");
        OnLeaderboardRatingReceived?.Invoke(LeaderboardRatingStatus.OK, leaderboardInfo);
        OnLeaderboardRatingReceived?.RemoveAllListeners();
    }

    public void RequestLB(string name,UnityAction<LeaderboardData> callback, bool includeUser=true,int quantityAround=1,int quantityTop=5)
    {
        OnLeaderboardDataReceived.AddListener(callback);
        RequestLeaderboard(name, includeUser, quantityAround, quantityTop);
    }

    public void YSCB_LeaderboardDataCallback(string json)
    {
        LeaderboardData data = YSDKJsonConverter.ConvertToLeaderboardData(json);
        OnLeaderboardDataReceived?.Invoke(data);
        OnLeaderboardDataReceived?.RemoveAllListeners();
    }
}
