using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class YandexLeaderboard : MonoBehaviour
{


    public UnityEvent<LeaderboardInfo> OnLeaderboardDescriptionReceived;
    public UnityEvent<bool> OnLeaderboardAvailableReceived;

    [DllImport("__Internal")]
    private static extern void AskLeaderboardDescription(string name);

    [DllImport("__Internal")]
    private static extern void AskLeaderboardAvailable();
    [DllImport("__Internal")]
    private static extern void AskSetLeaderboardScore(string name,int value);

    public void GetLeaderboardDescription(string name, UnityAction<LeaderboardInfo> callback)
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

    public void ReceiveLeaderboardDescription(string desc)
    {
        Debug.Log("YSDK: Received LD Description");
        LeaderboardInfo info = YSDKJsonConverter.ConvertToLeaderboardInfo(desc);
        OnLeaderboardDescriptionReceived?.Invoke(info);
        OnLeaderboardDescriptionReceived?.RemoveAllListeners();
    }

    public void ReceiveLeaderboardAvailable(bool available)
    {
        Debug.Log("YSDK: Received LD Available");
        OnLeaderboardAvailableReceived?.Invoke(available);
        OnLeaderboardAvailableReceived?.RemoveAllListeners();
    }

    public void SetLeaderboardScore(string name,int value)
    {
        AskSetLeaderboardScore(name, value);
    }
}
