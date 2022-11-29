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

[RequireComponent(typeof(YandexAds))]
[RequireComponent(typeof(YandexLeaderboard))]
public class YandexSDK : MonoBehaviour
{
    /// <summary>
    /// Current representation of Yandex SDK
    /// </summary>
    public static YandexSDK Current => _instance;
    /// <summary>
    /// Get current player data
    /// </summary>
    public YaPlayer Player;
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

    private static YandexSDK _instance;
    private YaDevice _device;
    private YaPlayer _player;
    private YandexAds _ads;
    private YandexLeaderboard _leaderboard;

    // EVENTS

    public UnityEvent<YaPlayer> OnPlayerDataChanged;
    public UnityEvent<RatingResult> OnRated;



    [DllImport("__Internal")] 
    private static extern void GetPlayerData();
    [DllImport("__Internal")] 
    private static extern int GetDeviceID();

    [DllImport("__Internal")]
    private static extern void DebugLog(string msg);

    [DllImport("__Internal")]
    private static extern void AskForRating();


    private void Start()
    {

        Debug.Log("Yandex SDK v0.0.2.2 starting...");

        if (Debug.isDebugBuild)
        {
            Debug.developerConsoleVisible = true;
        }
        if (_instance == null)
        {
            Debug.Log("Yandex SDK v0.0.2.2 initialized");
            _instance = this;
            _ads = GetComponent<YandexAds>();
            _leaderboard = GetComponent<YandexLeaderboard>();
            Debug.Log("Yandex ADS loaded    ");
            DontDestroyOnLoad(gameObject);
            DebugLog("Getting device info");
            _device = GetDevice();
            DebugLog("DeviceInfo stored");
        }
    }

    /// <summary>
    /// Load Yandex Player data from Yandex.SDK
    /// </summary>
    public void SetupPlayer()
    {
        _player = new YaPlayer();
        GetPlayerData();
    }

    /// <summary>
    /// WebGL callback for processing player data
    /// </summary>
    /// <param name="json">PlayerData</param>
    public void YSCB_ReceivePlayerData(string json)
    {
        Debug.Log("Recieved json from js:");
        Debug.Log(json);
        _player = JsonUtility.FromJson<YaPlayer>(json);
        OnPlayerDataChanged?.Invoke(_player);
    }


    private YaDevice GetDevice()
    {
        int deviceID = GetDeviceID();
        DebugLog($"Device get - {deviceID}");
        YaDevice device = (YaDevice)deviceID;
        DebugLog($"parsing - {device.ToString()}");
        return (YaDevice)deviceID;
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
    /// WebGL �allback for review result
    /// </summary>
    /// <param name="resultID">Result Code</param>
    public void YSCB_AskForRatingCallback(int resultID)
    {
        
        RatingResult result = (RatingResult)resultID;
        Debug.Log($"Rating callback return code: {resultID} - {result.ToString()}");
        OnRated?.Invoke(result);
        OnRated?.RemoveAllListeners();
    }
}
