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
    public static YandexSDK Current => _instance;
    public YaPlayer Player;
    public YaDevice Device => _device;
    public YandexAds Ads => _ads;
    public YandexLeaderboard Leaderboards => _leaderboard;

    private static YandexSDK _instance;
    private YaDevice _device;
    private YaPlayer _player;

    public UnityEvent<YaPlayer> OnPlayerDataChanged;
    public UnityEvent<RatingResult> OnRatingAsk;

    private YandexAds _ads;
    private YandexLeaderboard _leaderboard;

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

        Debug.Log("Yandex SDK v0.0.2.0 starting...");

        if (Debug.isDebugBuild)
        {
            Debug.developerConsoleVisible = true;
        }
        if (_instance == null)
        {
            Debug.Log("Yandex SDK v0.0.2.0 initialized");
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

    /*private void Start()
    {
        
    }*/

    public void SetupPlayer()
    {
        _player = new YaPlayer();
        GetPlayerData();
    }

    public void ReceivePlayerData(string json)
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

    public void RateGame()
    {
        AskForRating();
    }

    public void AskForRatingCallback(int resultID)
    {
        
        RatingResult result = (RatingResult)resultID;
        Debug.Log($"Rating callback return code: {resultID} - {result.ToString()}");
        OnRatingAsk?.Invoke(result);
    }
}
