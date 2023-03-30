mergeInto(LibraryManager.library, {

    // OBSOLETE
    GetPlayerData: function () {
        var playerJson;
        try {
            if (window.player.getMode() == 'lite') {
                // Игрок не авторизован.
                window.ysdk.auth.openAuthDialog().then(() => {
                    window.ysdk.getPlayer().then(_player => {
                        playerJson = {
                            "uID": window.player.getUniqueID(),
                            "name": window.player.getName(),
                            "auth": true,
                            "smallPhoto": window.player.getPhoto("small"),
                            "mediumPhoto": window.player.getPhoto("medium"),
                            "largePhoto": window.player.getPhoto("large")
                        }
                    });
                }).catch(() => {
                    playerJson = {
                        "uID": window.player.getUniqueID(),
                        "name": "Guest" + Math.floor(Math.random() * 500000),
                        "auth": false,
                        "smallPhoto": window.player.getPhoto("small"),
                        "mediumPhoto": window.player.getPhoto("medium"),
                        "largePhoto": window.player.getPhoto("large")
                    };
                });
            }
            else {
                playerJson = {
                    "uID": window.player.getUniqueID(),
                    "name": window.player.getName(),
                    "isAuth": true,
                    "smallPhoto": window.player.getPhoto("small"),
                    "mediumPhoto": window.player.getPhoto("medium"),
                    "largePhoto": window.player.getPhoto("large")
                };
            }
            window.unityInstance.SendMessage("YaSDK", "YSCB_ReceivePlayerData", JSON.stringify(playerJson));
        }
        catch (e) {
            console.warn("ERROR AUTH! " + e);
            console.error(e);
        }
    },

    GetPlayerDataDifferent: function () {
        var playerJson;
        window.ysdk.getPlayer().then(_player => {
            playerJson = {
                "uID": window.player.getUniqueID(),
                "name": window.player.getName(),
                "auth": true,
                "smallPhoto": window.player.getPhoto("small"),
                "mediumPhoto": window.player.getPhoto("medium"),
                "largePhoto": window.player.getPhoto("large")
            };
            window.unityInstance.SendMessage("YaSDK", "YSCB_ReceivePlayerData", JSON.stringify(playerJson));
        });
    },

    IsPlayerAuth: function () {
        window.ysdk.getPlayer().then(_player => {
            var result;
            if (_player.getMode() === 'lite') {
                result = 0;
            }
            else {
                result = 1;
            }
            window.unityInstance.SendMessage("YaSDK", "YSCB_ReceivePlayerAuthStatus", result);
        });
    },

    AuthPlayer: function () {
        var result;
        window.ysdk.auth.openAuthDialog().then(() => {
            window.ysdk.getPlayer().then(_player => {
                window.player = _player;
                result = 1;
                window.unityInstance.SendMessage("YaSDK", "YSCB_ReceivePlayerAuthStatus", result);
            });
        }).catch(() => {
            result = 0;
            window.unityInstance.SendMessage("YaSDK", "YSCB_ReceivePlayerAuthStatus", result);
        });
    },

    GetDeviceID: function () {
        switch (window.ysdk.deviceInfo.type) {
            case "desktop": { window.unityInstance.SendMessage("YaSDK", "YSCB_OnDeviceRecieved", 2); break; }
            case "mobile": { window.unityInstance.SendMessage("YaSDK", "YSCB_OnDeviceRecieved",1); break; }
            case "tablet": { window.unityInstance.SendMessage("YaSDK", "YSCB_OnDeviceRecieved", 3); break; }
            case "tv": { window.unityInstance.SendMessage("YaSDK", "YSCB_OnDeviceRecieved", 4); break; }
            default: { window.unityInstance.SendMessage("YaSDK", "YSCB_OnDeviceRecieved", 0); break; }
        }
    },

    DebugLog: function (msg) {
        console.log("UnityGame: " + msg);
    },

    ShowRewardAdvert: function () {
        window.ysdk.adv.showRewardedVideo({
            callbacks: {
                onRewarded: () => {
                    window.unityInstance.SendMessage("YaSDK", "YSCB_RewardAdResult", 0);
                },
                onClose: () => {
                    window.unityInstance.SendMessage("YaSDK", "YSCB_RewardAdResult", 1);
                },
                onError: (e) => {
                    window.unityInstance.SendMessage("YaSDK", "YSCB_RewardAdResult", 2);
                }
            }
        })
    },

    AskForRating: function () {
        window.ysdk.feedback.canReview()
            .then(({ value, reason }) => {
                if (value) {
                    window.ysdk.feedback.requestReview()
                        .then(({ feedbackSent }) => {
                            if (feedbackSent) {
                                window.unityInstance.SendMessage("YaSDK", "YSCB_RatedResult", 5);
                            }
                            else {
                                window.unityInstance.SendMessage("YaSDK", "YSCB_RatedResult", 4);
                            }
                        })
                } else {
                    var callbackCode = 4;
                    switch (reason) {
                        case "NO_AUTH": { callbackCode = 0; break; }
                        case "GAME_RATED": { callbackCode = 1; break; }
                        case "REVIEW_ALREADY_REQUESTED": { callbackCode = 2; break; }
                        case "REVIEW_WAS_REQUESTED": { callbackCode = 3; break; }
                    }
                    window.unityInstance.SendMessage("YaSDK", "YSCB_AskForRatingCallback", callbackCode);
                }
            });
    },

    AskLeaderboardDescription: function (rawNameStr) {
        var name = UTF8ToString(rawNameStr);
        window.ysdk.getLeaderboards()
            .then(lb => {
                lb.getLeaderboardDescription(name)
                    .then(res => {
                        var json = JSON.stringify(res);
                        window.unityInstance.SendMessage("YaSDK", "YSCB_ReceiveLeaderboardDescription", json);
                    });
            });
    },

    AskLeaderboardAvailable: function () {
        window.ysdk.isAvailableMethod('leaderboards.setLeaderboardScore').
            then(res => {
                window.unityInstance.SendMessage("YaSDK", "YSCB_ReceiveLeaderboardAvailable", res);
            });
    },

    AskSetLeaderboardScore: function (rawNameStr, value) {
        var lbname = UTF8ToString(rawNameStr);
        window.ysdk.getLeaderboards()
            .then(lb => lb.getLeaderboardPlayerEntry(lbname))
            .then(res => {
                var json = {
                    "playerName": res.player.publicName,
                    "imageURL": res.player.getAvatarSrc("small"),
                    "rank": res.rank,
                    "score": res.score
                };
                if (value > res.score) {
                    window.ysdk.getLeaderboards()
                        .then(lb => {
                            lb.setLeaderboardScore(lbname, value);
                        });
                }
            })
            .catch(err => {
                if (err.code === 'LEADERBOARD_PLAYER_NOT_PRESENT') {
                    window.ysdk.getLeaderboards()
                        .then(lb => {
                            lb.setLeaderboardScore(lbname, value);
                        });
                }
                else {
                    console.log(err);
                }
            });
    },

    AskPlayerLeaderboardRating: function (rawNameStr) {
        var lbname = UTF8ToString(rawNameStr);
        window.ysdk.getLeaderboards()
            .then(lb => lb.getLeaderboardPlayerEntry(lbname))
            .then(res => {
                var json = {
                    "playerName": res.player.publicName,
                    "imageURL": res.player.getAvatarSrc("small"),
                    "rank": res.rank,
                    "score": res.score
                };
                window.unityInstance.SendMessage("YaSDK", "YSCB_LeaderboardRatingCallback", JSON.stringify(json));
            })
            .catch(err => {
                if (err.code === "LEADERBOARD_PLAYER_NOT_PRESENT") {
                    window.unityInstance.SendMessage("YaSDK", "YSCB_LeaderboardRatingCallback", "LEADERBOARD_PLAYER_NOT_PRESENT");
                }
                else {
                    console.log(err);
                }
            });
    },

    RequestLeaderboard: function (rawNameStr, includeUser, quantityAround, quantityTop) {
        var lbname = UTF8ToString(rawNameStr);
        ysdk.getLeaderboards()
            .then(lb => {
                lb.getLeaderboardEntries(lbname, { quantityTop: quantityTop, includeUser: includeUser, quantityAround: quantityAround })
                    .then(res => {
                        var lbAnswer = {
                            "lbName_ru": res.leaderboard.title.ru,
                            "lbName_en": res.leaderboard.title.en,
                            "playerRank": res.userRank,
                            "entries": []
                        };
                        var lbEntries = [];
                        res.entries.forEach(line => {
                            var entry = {
                                "playerName": line.player.publicName,
                                "imageURL": line.player.getAvatarSrc('small'),
                                "rank": line.line,
                                "score": line.score
                            };
                            lbEntries.push(entry);
                        });
                        lbAnswer.entries = lbEntries;
                        window.unityInstance.SendMessage("YaSDK", "YSCB_LeaderboardDataCallback", JSON.stringify(lbAnswer));
                    });
            });
    },

    RequestBannerStatus: function () {
        window.ysdk.adv.getBannerAdvStatus().then(({ stickyAdvIsShowing, reason }) => {
            if (stickyAdvIsShowing) {
                window.unityInstance.SendMessage("YaSDK", "YSCB_StickyBannerResult", 1);
            } else if (reason) {
                console.error("StickyBanner error - " + reason);
                if (reason == "ADV_IS_NOT_CONNECTED ") {
                    window.unityInstance.SendMessage("YaSDK", "YSCB_StickyBannerResult", 2);
                }
                else {
                    window.unityInstance.SendMessage("YaSDK", "YSCB_StickyBannerResult", 3);
                }
            } else {
                window.unityInstance.SendMessage("YaSDK", "YSCB_StickyBannerResult", 0);
            }
        });
    },

    ShowBanner: function () {
        try {
            window.ysdk.adv.showBannerAdv();
        }
        catch (e) {
            console.error(e);
        }
    },

    HideBanner: function () {
        try {
            window.ysdk.adv.hideBannerAdv();
        }
        catch(e) {
            console.error(e);
        }
    },
        

    ShowAdvert: function () {
        window.ysdk.adv.showFullscreenAdv({
            callbacks: {
                onClose: function (wasShown) {
                    if (wasShown) {
                        window.unityInstance.SendMessage("YaSDK", "YSCB_AdvertResult", 1);
                    }
                    else {
                        window.unityInstance.SendMessage("YaSDK", "YSCB_AdvertResult", 0);
                    }
                },
                onError: function (error) {
                    window.unityInstance.SendMessage("YaSDK", "YSCB_AdvertResult", 2);
                },
                onOffline: function () {
                    window.unityInstance.SendMessage("YaSDK", "YSCB_AdvertResult", 3);
                }
            }
        });
    },

    SetPlayerData: function (data) {
        var jsondata = UTF8ToString(data);
        window.player.setData(JSON.parse(jsondata),"true").then(() => {
        });
    },

    GetPlayerData: function () {
        window.player.getData().then((result) => {
            window.unityInstance.SendMessage("YaSDK", "YSDK_OnRecivePlayerData", JSON.stringify(result));
        });
    }
});