mergeInto(LibraryManager.library, {

    Check: function () {
        window.alert("Hello, world!");
    },

    GetPlayerData: function () {
        var playerJson;
        console.log("Getting info from YSDK");
        try {
            if (window.player.getMode() === 'lite') {
                // Игрок не авторизован.
                window.ysdk.auth.openAuthDialog().then(() => {
                    playerJson = {
                        "uID": window.player.getUniqueID(),
                        "name": window.player.getName(),
                        "auth": true,
                        "smallPhoto": window.player.getPhoto("small"),
                        "mediumPhoto": window.player.getPhoto("medium"),
                        "largePhoto": window.player.getPhoto("large")
                    };
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
                console.log("Sending info");
                playerJson = {
                    "uID": window.player.getUniqueID(),
                    "name": window.player.getName(),
                    "isAuth": true,
                    "smallPhoto": window.player.getPhoto("small"),
                    "mediumPhoto": window.player.getPhoto("medium"),
                    "largePhoto": window.player.getPhoto("large")
                };
            }
            console.log(JSON.stringify(playerJson));
            window.unityInstance.SendMessage("YaSDK", "YSCB_ReceivePlayerData", JSON.stringify(playerJson));
        }
        catch (e) {
            console.error(e);
        }
        console.log("Finish");
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
                    console.log("Showed: Rewarded");
                    window.unityInstance.SendMessage("YaSDK", "YSCB_RewardAdResult", 0);
                },
                onClose: () => {
                    console.log("Showed: Closed");
                    window.unityInstance.SendMessage("YaSDK", "YSCB_RewardAdResult", 1);
                },
                onError: (e) => {
                    console.log("Not Showed: Error");
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
                                window.unityInstance.SendMessage("YaSDK", "YSCB_AskForRatingCallback", 5);
                            }
                            else {
                                window.unityInstance.SendMessage("YaSDK", "YSCB_AskForRatingCallback", 4);
                            }
                        })
                } else {
                    console.log(reason)
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
        console.log("YSDKjslb: Received ASK from unity. Description of LB named " + name);
        window.ysdk.getLeaderboards()
            .then(lb => {
                console.log(lb);
                console.log("YSDKjslb: ysdk received answer, asking description for " + name);
                lb.getLeaderboardDescription(name)
                    .then(res => {
                        console.log("YSDKjslb: We have description now, sending json to unity...");
                        var json = JSON.stringify(res);
                        console.log("Sending: " + json);
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
                console.log(JSON.stringify(json));
                window.unityInstance.SendMessage("YaSDK", "YSCB_LeaderboardRatingCallback", JSON.stringify(json));
            })
            .catch(err => {
                if (err.code === 'LEADERBOARD_PLAYER_NOT_PRESENT') {
                    console.log("LEADERBOARD_PLAYER_NOT_PRESENT detected");
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
                        console.log(JSON.stringify(lbAnswer)); 
                        window.unityInstance.SendMessage("YaSDK", "YSCB_LeaderboardDataCallback", JSON.stringify(lbAnswer));
                    });
            });
    },
        

    ShowAdvert: function () {
        window.ysdk.adv.showFullscreenAdv({
            callbacks: {
                onClose: function (wasShown) {
                    if (wasShown) {
                        console.log("Showed: Delayed");
                        window.unityInstance.SendMessage("YaSDK", "YSCB_AdvertResult", 1);
                    }
                    else {
                        console.log("Showed: Success");
                        window.unityInstance.SendMessage("YaSDK", "YSCB_AdvertResult", 0);
                    }
                },
                onError: function (error) {
                    console.log("Not Showed: Error");
                    window.unityInstance.SendMessage("YaSDK", "YSCB_AdvertResult", 2);
                },
                onOffline: function () {
                    console.log("Not Showed: Offline");
                    window.unityInstance.SendMessage("YaSDK", "YSCB_AdvertResult", 3);
                }
            }
        });
    }
});