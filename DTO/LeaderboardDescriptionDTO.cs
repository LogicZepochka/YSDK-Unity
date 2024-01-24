using System;
using System.Collections.Generic;
using System.Text;

namespace Logzep.YandexSDK.DTO
{
    [Serializable]
    public class LeaderboardDescriptionDTO
    {
        public bool Default;
        public bool InvertSortOrder;
        public string Type;
        public string Name;
        public List<string> LocNameMap;
    }
}
