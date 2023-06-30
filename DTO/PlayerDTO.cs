using System;
using System.Collections.Generic;
using System.Text;

namespace Logzep.YandexSDK.DTO
{
    [Serializable]
    public class PlayerDTO
    {
        public string name;
        public string uid;
        public string sphoto;
        public string mphoto;
        public string lphoto;
        public bool auth;
    }
}
