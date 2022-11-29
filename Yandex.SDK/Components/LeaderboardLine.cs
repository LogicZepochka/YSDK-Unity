using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum LeaderboardScoreType
{
    Numeric,
    Time
}

public enum LeaderboardTimeScoreType
{
    Milliseconds,
    Seconds,
    Minutes
}

[RequireComponent(typeof(PlayerURLImage))]
public class LeaderboardLine : MonoBehaviour
{
    public LeaderboardScoreType ScoreType;
    public LeaderboardTimeScoreType TimeScoreType;

    public TMP_Text RankText;
    public TMP_Text NameText;
    public TMP_Text ScoreText;

    private int _rank;
    private int _score;
    private string _name;
    public PlayerURLImage _playerImage;

    public void LoadData(LeaderboardEntry data)
    {
        _rank = data.rank;
        _name = data.playerName;
        _score = data.score;


        if (_rank != -1)
        {
            RankText.text = $"#{_rank}";
        }
        else
        {
            if(RankText != null)
                RankText.text = "";
        }
        NameText.text = _name;

        if(ScoreType == LeaderboardScoreType.Numeric)
        {
            ScoreText.text = _score.ToString();
        }
        else
        {
            int showResult = 0;
            switch(TimeScoreType)
            {
                case LeaderboardTimeScoreType.Milliseconds:
                    {
                        showResult = _score;
                        break;
                    }
                case LeaderboardTimeScoreType.Seconds:
                    {
                        showResult = _score / 1000;
                        break;
                    }
                case LeaderboardTimeScoreType.Minutes:
                    {
                        showResult = _score / 1000 / 60;
                        break;
                    }
            }
            ScoreText.text = _score.ToString();
            _playerImage.LoadURLImage(data.imageURL);
        }
    }
}
