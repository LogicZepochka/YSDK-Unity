using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DemoScript : MonoBehaviour
{

    public TMP_Text Device;
    public TMP_Text AdsText;

    public PlayerURLImage PlayerImage;
    public TMP_Text PlayerName;

    public void OnPlayerDataUpdated(YaPlayer player)
    {
        Debug.Log("Checking YaPlayer parsing...");
        Debug.Log(player.Name);
        Debug.Log(player.UID);
        Debug.Log(player.SmallPhotoURL);
        Debug.Log("Checking YaPlayer Finish");
        PlayerName.text = player.Name;
        PlayerImage.LoadURLImage(player.SmallPhotoURL);
    }

    public void UpdateDeviceData()
    {
        string result;
        switch (YandexSDK.Current.Device)
        {
            case YaDevice.Desktop: { result = "Desktop"; break; }
            case YaDevice.Mobile: { result = "Mobile"; break; }
            case YaDevice.Tabled: { result = "Tabled"; break; }
            case YaDevice.TV: { result = "TV"; break; }
            default:
                {
                    result = "Unknown";
                    break;
                }
        }
        Device.text = result;
    }

    public void ShowAd()
    {
        YandexSDK.Current.Ads.ShowClassicAd(OnAdvertShown);
    }   

    public void ShowRewardedAd()
    {
        YandexSDK.Current.Ads.ShowRewardedAd(OnRewardedAdvertShown);
    }

    public void OnAdvertShown(AdResult result)
    {
        switch(result)
        {
            case AdResult.OK:
                {
                    AdsText.text = "Ad showed";
                    break;
                }
            case AdResult.Error:
                {
                    AdsText.text = "Ad ERROR";
                    break;
                }
            case AdResult.Offline:
                {
                    AdsText.text = "Ad offline";
                    break;
                }
        }
    }

    public void OnRewardedAdvertShown(RewardedAdResult result)
    {
        switch (result)
        {
            case RewardedAdResult.Rewarded:
                {
                    AdsText.text = "Ad rewarded";
                    break;
                }
            case RewardedAdResult.Error:
                {
                    AdsText.text = "Ad ERROR";
                    break;
                }
            case RewardedAdResult.Closed:
                {
                    AdsText.text = "Ad closed";
                    break;
                }
        }
    }

    public void RateGame()
    {
        YandexSDK.Current.RateGame();
    }

    public void ShowLeaderboardScreen()
    {
        YandexSDK.Current.Leaderboards.GetLeaderboardDescription("testleaderboard", OnLeaderboardRecievie);
    }

    private void OnLeaderboardRecievie(LeaderboardInfo leaderboardInfo)
    {
        Debug.Log(leaderboardInfo.title.ru);
        Debug.Log(leaderboardInfo.appID);
        Debug.Log(leaderboardInfo.description.score_format.options.decimal_offset);
    }
}
