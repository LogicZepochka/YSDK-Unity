using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class YandexSDKDataProvider : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SetPlayerData(string json);

    [DllImport("__Internal")]
    private static extern void GetPlayerData();

    public UnityEvent<string> OnRecivePlayerData;

    public void YSDK_OnRecivePlayerData(string json)
    {
        OnRecivePlayerData?.Invoke(json);
        OnRecivePlayerData?.RemoveAllListeners();
    }

    public void SetData(string DataToSave)
    {
        SetPlayerData(DataToSave);
    }

    public void GetData(UnityAction<string> callback)
    {
        GetPlayerData();
        OnRecivePlayerData.AddListener(callback);
    }
}
