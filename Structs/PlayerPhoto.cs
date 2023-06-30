using System;
using System.Collections.Generic;
using System.Text;

namespace Logzep.YandexSDK.Player
{
    public struct PlayerPhoto
    {
        public string GetLargePhoto => _largePhoto;
        public string GetSmallPhoto => _smallPhoto;
        public string GetMediumPhoto => _mediumPhoto;

        private string _smallPhoto;
        private string _mediumPhoto;
        private string _largePhoto;

        public PlayerPhoto(string smallPhoto, string mediumPhoto, string largePhoto)
        {
            _smallPhoto = smallPhoto;
            _mediumPhoto = mediumPhoto;
            _largePhoto = largePhoto;
        }
    }
}
