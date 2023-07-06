using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Logzep.YandexSDK.Player
{
    public class YandexPlayerData
    {
        public bool isLoaded => _isLoaded;
        public UnityEvent<YandexPlayerData> OnPlayerDataUpdated = new UnityEvent<YandexPlayerData>();

        private bool _isLoaded = false;

        private Dictionary<string, string> _playerData = new Dictionary<string, string>();
        private Dictionary<string, int> _playerStats = new Dictionary<string, int>();

        public void LoadDataFromYandex(string[] keys)
        {
            //YandexSDK.LoadPlayerData(keys);
        }

        public void LoadDataFromYandex(string key)
        {
            //string[] keys = { key };
            //LoadDataFromYandex(keys);
        }

        public void SaveDataToYandex()
        {

        }

        public void SaveData(string key, string value)
        {
            if(_playerData.ContainsKey(key))
            {
                _playerData[key] = value;
            }
            else
            {
                _playerData.Add(key, value);
            }
            OnPlayerDataUpdated?.Invoke(this);
        }

        public void SaveData(string[] keys, string[] values)
        {
            for(int i=0; i<keys.Length; i++)
            {
                if (_playerData.ContainsKey(keys[i]))
                {
                    _playerData[keys[i]] = values[i];
                }
                else
                {
                    _playerData.Add(keys[i], values[i]);
                }
            }
            OnPlayerDataUpdated?.Invoke(this);
        }

        public string GetData(string key)
        {
            if(!_playerData.ContainsKey(key)) {
                throw new ArgumentNullException(nameof(key));
            }
            return _playerData[key];
        }

        public void SaveStats(string key, int value)
        {
            if (_playerStats.ContainsKey(key))
            {
                _playerStats[key] = value;
            }
            else
            {
                _playerStats.Add(key, value);
            }
        }

        public int GetStats(string key)
        {
            if (!_playerStats.ContainsKey(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _playerStats[key];
        }

    }
}
