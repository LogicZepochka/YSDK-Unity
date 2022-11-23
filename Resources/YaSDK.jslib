mergeInto(LibraryManager.library, {

    Check: function () {
        window.alert("Hello, world!");
    },

    GetPlayerData: function () {
        console.log("Getting info from YSDK");
        try {
            if (window.player.getMode() === 'lite') {
                // Игрок не авторизован.
                window.ysdk.auth.openAuthDialog().then(() => {
                    console.log("Sending info");
                    window.unityInstance.SendMessage("YaSDK", "SetupName", window.player.getName());
                    window.unityInstance.SendMessage("YaSDK", "SetupImgUrl", window.player.getPhoto("small"));
                    window.unityInstance.SendMessage("YaSDK", "SetupUID", window.player.getUniqueID());
                }).catch(() => {
                    console.log("Sending info with nulls");
                    window.unityInstance.SendMessage("YaSDK", "SetupName", window.player.getName());
                    window.unityInstance.SendMessage("YaSDK", "SetupImgUrl", window.player.getPhoto("small"));
                    window.unityInstance.SendMessage("YaSDK", "SetupUID", window.player.getUniqueID());
                });
            }
            else {
                console.log("Sending info");
                window.unityInstance.SendMessage("YaSDK", "SetupName", window.player.getName());
                window.unityInstance.SendMessage("YaSDK", "SetupImgUrl", window.player.getPhoto("small"));
                window.unityInstance.SendMessage("YaSDK", "SetupUID", window.player.getUniqueID());
            }
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
        console.log("UnityGame: " +msg);
    },

    ShowRewardAdvert: function() {
        window.ysdk.adv.showRewardedVideo({
            callbacks: {
                onRewarded: () => {
                    console.log("Showed: Rewarded");
                    window.unityInstance.SendMessage("YaSDK", "ReciveRewardAdResult", 0);
                },
                onClose: () => {
                    console.log("Showed: Closed");
                    window.unityInstance.SendMessage("YaSDK", "ReciveRewardAdResult", 1);
                },
                onError: (e) => {
                    console.log("Not Showed: Error");
                    window.unityInstance.SendMessage("YaSDK", "ReciveRewardAdResult", 2);
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
                                window.unityInstance.SendMessage("YaSDK", "AskForRatingCallback", 5);
                            }
                            else {
                                window.unityInstance.SendMessage("YaSDK", "AskForRatingCallback", 4);
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
                    window.unityInstance.SendMessage("YaSDK", "AskForRatingCallback", callbackCode);
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
                        window.unityInstance.SendMessage("YaSDK", "ReceiveLeaderboardDescription", json);
                    });
            });
    },

    AskLeaderboardAvailable: function () {
        window.ysdk.isAvailableMethod('leaderboards.setLeaderboardScore').
            then(res => {
                window.unityInstance.SendMessage("YaSDK", "ReceiveLeaderboardAvailable", res);
            });
    },

    AskSetLeaderboardScore: function (name,value) {
        window.ysdk.getLeaderboards()
            .then(lb => {
                lb.setLeaderboardScore(name, value);
            });
    },

    ShowAdvert: function () {
        window.ysdk.adv.showFullscreenAdv({
            callbacks: {
                onClose: function (wasShown) {
                    if (wasShown) {
                        console.log("Showed: Delayed");
                        window.unityInstance.SendMessage("YaSDK", "ReciveAdvertResult", 1);
                    }
                    else {
                        console.log("Showed: Success");
                        window.unityInstance.SendMessage("YaSDK", "ReciveAdvertResult", 0);
                    }
                },
                onError: function (error) {
                    console.log("Not Showed: Error");
                    window.unityInstance.SendMessage("YaSDK", "ReciveAdvertResult", 2);
                },
                onOffline: function () {
                    console.log("Not Showed: Offline");
                    window.unityInstance.SendMessage("YaSDK", "ReciveAdvertResult", 3);
                }
            }
        });
    }
});