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