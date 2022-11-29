using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    public Transform LeaderboardPanel;
    public GameObject LeaderboardLinePrefab;
    public LeaderboardLine InfoAboutPlayer;
    public TMP_Text Title;

    public List<GameObject> RatingLines = new List<GameObject>();

    public void UpdateLeaderboard(string name)
    {
        YandexSDK.Current.Leaderboards.GetLeaderboardRating(name, OnRatingGet);
        YandexSDK.Current.Leaderboards.RequestLB(name, OnLBRatingGet);
    }

    private void OnLBRatingGet(LeaderboardData data)
    {
        Title.text = data.lbName_ru;
        foreach(var obj in RatingLines)
        {
            Destroy(obj);
        }
        RatingLines.Clear();
        foreach (var entry in data.entries)
        {
            var newLine = Instantiate(LeaderboardLinePrefab, LeaderboardPanel);
            LeaderboardLine lineInfo = newLine.GetComponent<LeaderboardLine>();
            RatingLines.Add(newLine);
            lineInfo.LoadData(entry);
        }
    }

    private void OnRatingGet(LeaderboardRatingStatus status, LeaderboardEntry rating)
    {
        Debug.Log("Loading current player rating...");
        Debug.Log(rating.playerName);
        Debug.Log(rating.score);
        Debug.Log(rating.imageURL);
        if (status == LeaderboardRatingStatus.PlayerNotPresent)
        {
            InfoAboutPlayer.gameObject.SetActive(false);
        }
        else
        {
            InfoAboutPlayer.gameObject.SetActive(true);
            InfoAboutPlayer.LoadData(rating);
        }
    }
}
