mergeInto(LibraryManager.library, {

    GetPlayerData: function () {
        var playerJson;
        try {
            if (window.player.getMode() === 'lite') {
                // Игрок не авторизован.
                window.ysdk.auth.openAuthDialog().then(() => {
                    console.warn("Player Login Succes Check Send Data");
                    playerJson = {
                        "uID": window.player.getUniqueID(),
                        "name": window.player.getName(),
                        "auth": true,
                        "smallPhoto": window.player.getPhoto("small"),
                        "mediumPhoto": window.player.getPhoto("medium"),
                        "largePhoto": window.player.getPhoto("large")
                    };
                }).catch(() => {
                    console.warn("Player NotLogin Check Send Data");
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
            console.warn("Player Login Check Send Data");
            window.unityInstance.SendMessage("YaSDK", "YSCB_ReceivePlayerData", JSON.stringify(playerJson));
        }
        catch (e) {
            console.error(e);
        }
    },

    GetDeviceID: function () {
        switch (window.deviceType) {
            case "desktop": { return 2; break; }
            case "mobile": { return 1; break; }
            case "tablet": { return 3; break; }
            case "tv": { return 4; break; }
            default: { return 0; break; }
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
            .then(lb => {
                lb.setLeaderboardScore(lbname, value);
            });
    },

    AskPlayerLeaderboardRating: function (rawNameStr) {
        var lbname = UTF8ToString(rawNameStr);
        console.log(lbname);
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
                if (err.code === 'LEADERBOARD_PLAYER_NOT_PRESENT') {
                    window.unityInstance.SendMessage("YaSDK", "YSCB_LeaderboardRatingCallback", res);
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
            console.log("Sticky Banner Showing...");
            window.ysdk.adv.showBannerAdv();
            console.log("Sticky Banner Showed...");
        }
        catch (e) {
            console.error(e);
        }
    },

    HideBanner: function () {
        try {
            console.log("Sticky Banner Hiding...");
            window.ysdk.adv.hideBannerAdv();
            console.log("Sticky Banner Hided...");
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
    }
});