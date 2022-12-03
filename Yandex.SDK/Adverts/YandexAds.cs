﻿using UnityEngine.Events;
using UnityEngine;
using System.Runtime.InteropServices;


public class YandexAds: MonoBehaviour
{
    [Header("UnityEvent on Classic Ads")]
    private UnityEvent<AdResult> OnAdvertShown;
    [Header("UnityEvent on Reward Ads")]
    private UnityEvent<RewardedAdResult> OnRewardAdvertShown;
    private UnityEvent<StickyBannerStatus> OnBannerResultRecieved;


    [DllImport("__Internal")]
    private static extern int ShowBanner();
    [DllImport("__Internal")]
    private static extern int HideBanner();
    [DllImport("__Internal")] 
    private static extern int ShowAdvert();
    [DllImport("__Internal")]
    private static extern int ShowRewardAdvert();
    [DllImport("__Internal")]
    private static extern int RequestBannerStatus();

    /// <summary>
    /// WebGL callback to get Yandex Ad result
    /// </summary>
    /// <param name="resultID">Result Code</param>
    public void YSCB_AdvertResult(int resultID)
    {
        AdResult result = AdResult.Error;
        result = (AdResult)resultID;
        OnAdvertShown?.Invoke(result);
        OnAdvertShown?.RemoveAllListeners();
    }

    public void YSCB_StickyBannerResult(int resultID)
    {
        StickyBannerStatus result = (StickyBannerStatus)resultID;
        OnBannerResultRecieved?.Invoke(result);
        OnBannerResultRecieved?.RemoveAllListeners();
    }

    /// <summary>
    /// WebGL callback to get Yandex Rewarded Ad result
    /// </summary>
    /// <param name="resultID">Result Code</param>
    public void YSCB_RatedResult(int resultID)
    {
        RewardedAdResult result = RewardedAdResult.Error;
        result = (RewardedAdResult)resultID;
        OnRewardAdvertShown?.Invoke(result);
        OnRewardAdvertShown?.RemoveAllListeners();
    }


    /// <summary>
    /// Show standard ads that the user can close
    /// </summary>
    /// <param name="callback">Сallback to be called after showing</param>
    public void ShowClassicAd(UnityAction<AdResult> callback)
    {
        OnAdvertShown?.AddListener(callback);
        ShowAdvert();
    }

    public void GetStickyBannerStatus(UnityAction<StickyBannerStatus> callback)
    {
        OnBannerResultRecieved.AddListener(callback);
        RequestBannerStatus();
    }

    /// <summary>
    /// Show standard ads that the user can close
    /// </summary>
    public void ShowClassicAd()
    {
        ShowAdvert();
    }

    /// <summary>
    /// Show rewarded ads that the user can't close
    /// </summary>
    /// <param name="rewardCallback">Callback after the ad has finished showing</param>
    public void ShowRewardedAd(UnityAction<RewardedAdResult> rewardCallback)
    {
        OnRewardAdvertShown?.AddListener(rewardCallback);
        ShowRewardAdvert();
    }
}
