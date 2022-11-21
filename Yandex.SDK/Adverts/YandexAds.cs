using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;
using System.Runtime.InteropServices;


public class YandexAds: MonoBehaviour
{
    [Header("UnityEvent on Classic Ads")]
    public UnityEvent<AdResult> OnAdvertShown;
    [Header("UnityEvent on Reward Ads")]
    public UnityEvent<RewardedAdResult> OnRewardAdvertShown;

    [DllImport("__Internal")]
    private static extern int ShowAdvert();
    [DllImport("__Internal")]
    private static extern int ShowRewardAdvert();

    public void ReciveAdvertResult(int resultID)
    {
        AdResult result = AdResult.Error;
        result = (AdResult)resultID;
        OnAdvertShown?.Invoke(result);
        OnAdvertShown?.RemoveAllListeners();
    }

    public void ReciveRewardAdResult(int resultID)
    {
        RewardedAdResult result = RewardedAdResult.Error;
        result = (RewardedAdResult)resultID;
        OnRewardAdvertShown?.Invoke(result);
        OnRewardAdvertShown?.RemoveAllListeners();
    }

    public void ShowClassicAd(UnityAction<AdResult> callback)
    {
        OnAdvertShown?.AddListener(callback);
        ShowAdvert();
    }

    public void ShowClassicAd()
    {
        ShowAdvert();
    }

    public void ShowRewardedAd(UnityAction<RewardedAdResult> rewardCallback)
    {
        OnRewardAdvertShown?.AddListener(rewardCallback);
        ShowRewardAdvert();
    }

    public void ShowRewardedAd()
    {   
        ShowRewardAdvert();
    }
}
