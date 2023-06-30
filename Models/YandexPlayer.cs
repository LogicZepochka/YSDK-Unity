using Logzep.YandexSDK.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logzep.YandexSDK.Player
{
    public class YandexPlayer
    {
        /// <summary>
        /// Авторизирован ли игрок
        /// </summary>
        public bool IsAutorized => _isAutorized;
        /// <summary>
        /// Получить имя игрока. Если игрок не авторизован - вернет пустую строку
        /// </summary>
        public string GetName => _name;
        /// <summary>
        /// Получить уникальный идентификатор игрока
        /// </summary>
        public string GetUID => _userID;
        /// <summary>
        /// Получить URL ссылки на аватарки игрока
        /// </summary>
        public PlayerPhoto GetPlayerPhoto => _playerPhoto;

        private bool _isAutorized;

        // PlayerData
        private string _name;
        private string _userID;
        private PlayerPhoto _playerPhoto;

        public YandexPlayer(PlayerDTO playerDto)
        {
            _isAutorized = playerDto.auth;
            _name = playerDto.name;
            _userID = playerDto.uid;
            _playerPhoto = new PlayerPhoto(playerDto.sphoto, playerDto.mphoto, playerDto.lphoto);
        }
    }
}
