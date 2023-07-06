using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Logzep.YandexSDK
{
    public enum DeviceInfo
    {
        Desktop,
        Mobile,
        Tablet,
        TV,
        Unknown = 10
    }

    [Serializable]
    public class Environment
    {
        public DeviceInfo DeviceInfo => (DeviceInfo)deviceID;

        public string id;
        public string payload;
        public string Lang;
        public int deviceID;

        public Environment(string json)
        {
            Environment temp = JsonUtility.FromJson<Environment>(json);
            id = temp.id;
            payload = temp.payload;
            Lang = temp.Lang;
            deviceID = temp.deviceID;
        }
    }
}
