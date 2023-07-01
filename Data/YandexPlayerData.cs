using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Logzep.YandexSDK.Player
{
    public abstract class YandexPlayerData
    {
        public bool isLoaded => _isLoaded;
        public UnityEvent<YandexPlayerData> OnPlayerDataUpdated = new UnityEvent<YandexPlayerData>();

        private bool _isLoaded = false;
        public void SaveData()
        {
            if (!YandexSDK.IsInitialized)
                throw new Exception("YandexSDK is not initialized!");
            string json = JsonUtility.ToJson(this);
            YandexSDK.SavePlayerData(json);
        }
    }
}
