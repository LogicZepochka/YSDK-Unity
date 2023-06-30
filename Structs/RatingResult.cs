using System;
using System.Collections.Generic;
using System.Text;

namespace Logzep.YandexSDK
{
    public enum RatingResult
    {
        NoAuth,
        GameRated,
        ReviewAlreadyRequested,
        ReviewWasRequested,
        Unknown,
        RefusedByPlayer,
        Success
    }
}
