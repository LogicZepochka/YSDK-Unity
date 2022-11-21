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
public class YandexSDK : MonoBehaviour
{
    public static YandexSDK Current => _instance;
    public YaPlayer Player;
    public YaDevice Device => _device;
    public YandexAds Ads => _ads;

    private static YandexSDK _instance;
    private YaDevice _device;
    private YaPlayer _player;

    public UnityEvent<YaPlayer> OnPlayerDataChanged;

    private YandexAds _ads;

    [DllImport("__Internal")] 
    private static extern void GetPlayerData();
    [DllImport("__Internal")] 
    private static extern int GetDeviceID();

    [DllImport("__Internal")]
    private static extern void DebugLog(string msg);

    private void Start()
    {

        Debug.Log("Yandex SDK v0.0.0.1 starting...");
        if (Debug.isDebugBuild)
        {
            Debug.developerConsoleVisible = true;
        }
        if (_instance == null)
        {
            Debug.Log("Yandex SDK v0.0.0.1 initialized");
            _instance = this;
            _ads = GetComponent<YandexAds>();
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

    public void SetupName(string name)
    {
        _player.SetName(name);
        OnPlayerDataChanged?.Invoke(_player);
    }

    public void SetupImgUrl(string url)
    {
        _player.SetImgUrl(url);
        OnPlayerDataChanged?.Invoke(_player);
    }

    public void SetupUID(string uid)
    {
        _player.SetUID(uid);
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
}
