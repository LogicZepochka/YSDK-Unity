using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public enum YaDevice
{
    Unknown,
    Mobile,
    Desktop,
    Tabled,
    TV
}

public enum YandexAutorizeStatus
{
    None,
    Autorized
}

[RequireComponent(typeof(YandexAds))]
[RequireComponent(typeof(YandexLeaderboard))]
[RequireComponent(typeof(YandexSDKDataProvider))]
public class YandexSDK : MonoBehaviour
{
    /// <summary>
    /// Current representation of Yandex SDK
    /// </summary>
    public static YandexSDK Current => _instance;
    /// <summary>
    /// Get current player data
    /// </summary>
    public YaPlayer Player => _player;
    /// <summary>
    /// Get current device
    /// </summary>
    public YaDevice Device => _device;
    /// <summary>
    /// Class for working with advertising
    /// </summary>
    public YandexAds Ads => _ads;
    /// <summary>
    /// Class for working with leaderboards
    /// </summary>
    public YandexLeaderboard Leaderboards => _leaderboard;

    public YandexSDKDataProvider DataProvider => _dataprovider;

    private static YandexSDK _instance;
    private YaDevice _device;
    private YaPlayer _player = new YaPlayer();
    private YandexAds _ads;
    private YandexLeaderboard _leaderboard;
    private YandexSDKDataProvider _dataprovider;

    // EVENTS

    public UnityEvent<YaPlayer> OnPlayerDataChanged;

    public UnityEvent<YandexAutorizeStatus> OnPlayerAuthStatusRecieved;
    public UnityEvent<RatingResult> OnRated;
    public UnityEvent OnReady;
    public UnityEvent OnBrowserClosed;


    [DllImport("__Internal")] 
    private static extern void GetPlayerData();


    [DllImport("__Internal")]
    private static extern void GetPlayerDataDifferent();
    [DllImport("__Internal")] 
    private static extern int GetDeviceID(); 
    [DllImport("__Internal")]
    private static extern void IsPlayerAuth();
    [DllImport("__Internal")]
    private static extern void AuthPlayer();

    [DllImport("__Internal")]
    private static extern void DebugLog(string msg);

    [DllImport("__Internal")]
    private static extern void AskForRating();


    private void Awake()
    {
        if (Debug.isDebugBuild)
        {
            Debug.developerConsoleVisible = true;
        }
        if (_instance == null)
        {
            Debug.Log("Unity YSDK v1.3.0 initialized");
            _instance = this;
            _ads = GetComponent<YandexAds>();
            _leaderboard = GetComponent<YandexLeaderboard>();
            _dataprovider = GetComponent<YandexSDKDataProvider>();
            DontDestroyOnLoad(gameObject);
            //GetDevice();
            Ads.ShowClassicAd();
        }
    }

    private void Start()
    {
        GetDevice();
        OnReady?.Invoke();
    }

    /// <summary>
    /// Load Yandex Player data from Yandex.SDK
    /// </summary>
    [System.Obsolete]
    public void SetupPlayer()
    {
        _player = new YaPlayer();
        GetPlayerData();
    }

    public void SetupYandexPlayer(UnityAction<YaPlayer> callback)
    {
        OnPlayerDataChanged?.AddListener(callback);
        _player = new YaPlayer();
        GetPlayerDataDifferent();
    }

    /// <summary>
    /// WebGL callback for processing player data
    /// </summary>
    /// <param name="json">PlayerData</param>
    public void YSCB_ReceivePlayerData(string json)
    {
        //Debug.Log(json);
        _player = JsonUtility.FromJson<YaPlayer>(json);
        //Debug.Log(_player.name);
        //Debug.Log(_player.smallPhoto);
        OnPlayerDataChanged?.Invoke(_player);
        OnPlayerDataChanged.RemoveAllListeners();
    }

    public void YSCB_OnBrowserClosed()
    {
        OnBrowserClosed?.Invoke();
    }

    public void RequestPlayerAuthStatus(UnityAction<YandexAutorizeStatus> callback)
    {
        OnPlayerAuthStatusRecieved.AddListener(callback);
        IsPlayerAuth();
    }

    public void RequestPlayerAuth(UnityAction<YandexAutorizeStatus> callback)
    {
        OnPlayerAuthStatusRecieved.AddListener(callback);
        AuthPlayer();
    }

    public void YSCB_ReceivePlayerAuthStatus(int statusID)
    {
        YandexAutorizeStatus status = (YandexAutorizeStatus)statusID;
        OnPlayerAuthStatusRecieved?.Invoke(status);
        OnPlayerAuthStatusRecieved?.RemoveAllListeners();
    }

    private void GetDevice()
    {
        GetDeviceID();
    }

    public void YSCB_OnDeviceRecieved(int id)
    {
        _device = (YaDevice)id;
    }

    /// <summary>
    /// Ask player for rating game
    /// </summary>
    public void RateGame()
    {
        AskForRating();
    }

    /// <summary>
    /// Ask player for rating game
    /// </summary>
    /// <param name="callback">Rating result</param>
    public void RateGame(UnityAction<RatingResult> callback)
    {
        OnRated.AddListener(callback);
        AskForRating();
    }

    /// <summary>
    /// WebGL ñallback for review result
    /// </summary>
    /// <param name="resultID">Result Code</param>
    public void YSCB_AskForRatingCallback(int resultID)
    {
        
        RatingResult result = (RatingResult)resultID;
        OnRated?.Invoke(result);    
        OnRated?.RemoveAllListeners();
    }
}
