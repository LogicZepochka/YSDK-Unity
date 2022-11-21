# YSDK-Unity
## _Интеграция SDK Яндекс.Игр в Unity_


## Содержание

- Готовый HTML код и библиотека _jslib_ для подключения к SDK Яндекса
- Набор скриптов для взаимодействия игры с SDK и наоборот

## Установка

1. Скачайте готовый [UnityPackage](https://github.com/LogicZepochka/YSDK-Unity/releases/tag/Unstable) и ипортируйте его в свой проект.
2. Добавьте префаб YaSDK на самую первую сцену вашей игры.

# Использование
Главным классом для работы с SDK Яндекс.Игр является __YandexSDK__
### Игрок ###
Перед получением данных игрока необходимо выполнить его инициализацию
```cs
YandexSDK.Current.SetupPlayer();
```
Игра выполнит запрос данных об игроке. Если он не был авторизирован, сервис Яндекс.Игры попросит его это сделать.
После получения завершения, получить данные игрока можно свойством
```
YandexSDK.Current.Player
```

### Работа с рекламой ###

За показ рекламы в игре отвечает класс __YandexAds__.

__Полноэкранный блок рекламы__
***

> Полноэкранный блок рекламы — блоки с рекламой, которые полностью закрывают фон приложения и показываются между запросом какой-то информации пользователем (например, при переходе на следующий уровень игры) и ее получением.
>
Вызов полноэкранного блока
```
YandexSDK.Current.Ads.ShowClassicAd();
```
__Видеореклама с вознаграждением__
***

> Видео с вознаграждением — блоки с видеорекламой, которые используются для монетизации игр. За просмотр видеоролика пользователь получает награду или внутриигровую валюту.
>
Вызов Видеореклама с вознаграждением
```
YandexSDK.Current.Ads.ShowRewardedAd();
```

### Обработка рекламмных событий ###
___Обработка полноэкранного блока рекламы___
__YandexAds.OnAdvertShown__ - отвечает за события показа рекламы.
Возвращает перечисление __AdResult__

Пример использования:
```
public void ShowAd()
{
    YandexSDK.Current.Ads.ShowClassicAd(OnAdvertShown); // Просим Яндекс показать рекламу, указав обработчик
}   

// Обработчик события
public void OnAdvertShown(AdResult result)
{
        switch(result)
        {
            case AdResult.OK: // Реклама была показана без ошибок
                {
                    ...
                    break;
                }
            case AdResult.Error: // Во время показа возникла ошибка
                {
                    ...
                    break;
                }
            case AdResult.Offline: // Игрок в офлайне
                {
                    ...
                    break;
                }
        }
}
```

___Обработка видеорекламы с вознаграждением___
__YandexAds.OnRewardAdvertShown__ - отвечает за события показа рекламы.
Возвращает перечисление __RewardedAdResult__

Пример использования:
```
public void ShowRewardedAd()
{
        YandexSDK.Current.Ads.ShowRewardedAd(OnRewardedAdvertShown); // Просим Яндекс показать рекламу и указываем обработчик
}
    
public void OnRewardedAdvertShown(RewardedAdResult result)
{
        switch (result)
        {
            case RewardedAdResult.Rewarded: // Пользователь досмотрел до конца
                {
                    ...
                    break;
                }
            case RewardedAdResult.Error: // Возникла ошибка при показе
                {
                    ...
                    break;
                }
            case RewardedAdResult.Closed: // Пользователь закрыл видеорекламу
                {
                    ...
                    break;
                }
        }
}
```
### Получение данных об устройстве ###
Получить текущее устройство:
```
YandexSDK.Current.Device
```

### Рейтинг ###
Для запроса оценки у игрока:
```
YandexSDK.Current.RateGame()
```
После этого вызова, игроку отобразится окно оценки игрока.
После оценки, будет вызвано событие OnRatingAsk c результатом:
- _NoAuth_ - игрок не был авторизирован
- _GameAlreadyRated_ - игрок уже оставлял оценку
- _ReviewAlreadyRequested_ - запрос уже открыт
- _ReviewWasRequested_ - игрока уже спрашивали
- _PlayerRejected_ - игрок отказался оценивать игру
- _PlayerRated_ - игрок поставил оценку
- _Unknown_ - ошибка


### События
___YandexSDK___
* _OnPlayerDataChanged_ - данные игрока изменлись

__YandexAds__
* _OnAdvertShown_ - показ рекламы завершен
* _OnRewardAdvertShown_ - показ видеорекламы с наградой завершен

***
## Скачать ##

Последняя версия: [__0.0.1__](https://github.com/LogicZepochka/YSDK-Unity/releases/tag/Unstable)
