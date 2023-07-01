mergeInto(LibraryManager.library, {

	InitFinish: function (gameName, version, ysdkVersion) {
		console.group("YSDK-UNITY START")
		console.log(`Game: ${UTF8ToString(gameName)}`)
		console.log(`Version: ${UTF8ToString(version)}`)
		console.log(`uYSDK-Version: ${UTF8ToString(ysdkVersion)}`)
		console.groupEnd();
		window.ysdk.features.LoadingAPI.ready();
	},

	InitStart: function () {
		window.YaGames.init().then(_sdk => {
			window.ysdk = _sdk;
			window.ysdk.getPlayer().then(_player => {
				window.player = _player;
				var playerObject = {
					name: "",
					uid: "",
					sphoto: "",
					mphoto: "",
					lphoto: "",
					auth: false
				};
				if (_player.getMode() === 'lite') {
					playerObject.name = "Player";
					playerObject.uid = _player.getUniqueID();
				}
				else {
					playerObject.name = _player.getName();
					playerObject.uid = _player.getUniqueID();
					playerObject.sphoto = _player.getPhoto('small');
					playerObject.mphoto = _player.getPhoto('medium');
					playerObject.lphoto = _player.getPhoto('large');
					playerObject.auth = true;
				}
				console.log("Sending:");
				console.log(playerObject);
				let jsonPlayer = JSON.stringify(playerObject);
				console.log(jsonPlayer);
				window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_InitPlayer", jsonPlayer);
			});
		}).catch(console.error);
	},

	Rate: function () {
		window.ysdk.feedback.canReview().then(({ value, reason }) => {
			let responce = 0;
			if (value) {
				ysdk.feedback.requestReview()
					.then(({ feedbackSent }) => {
						if (feedbackSent) {
							window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_RateResult", 6); // Success
						}
						else {
							window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_RateResult", 5); // RefusedByPlayer
						}
					});
			}
			else {
				switch (reason) {
					case 'NO_AUTH': window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_RateResult", 0); break;
					case 'GAME_RATED': window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_RateResult", 1); break;
					case 'REVIEW_ALREADY_REQUESTED': window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_RateResult", 2); break;
					case 'REVIEW_WAS_REQUESTED': window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_RateResult", 3); break;
					case 'UNKNOWN': window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_RateResult", 4); break;
				}
			}
		});
	},

	ShowAd: function () {
		window.ysdk.adv.showFullscreenAdv({
			callbacks: {
				onClose: function (wasShown) {
					window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_AdResult", 0);
				},
				onError: function (error) {
					window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_AdResult", 2);
				},
				onOpen: function () {
					window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_AdResult", 1);
				},
				onOffline: function () {
					window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_AdResult", 3);
				}
			}
		})
	},

	ShowRewardedAd: function () {
		window.ysdk.adv.showRewardedVideo({
			callbacks: {
				onOpen: () => {
					window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_RewardedAdResult", 1);
				},
				onRewarded: () => {
					window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_RewardedAdResult", 3);
				},
				onClose: () => {
					window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_RewardedAdResult", 0);
				},
				onError: (e) => {
					window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_RewardedAdResult", 2);
				}
			}
		})
	},

	LoadPlayerData: function () {
		window.player.getData().then((data) => {
			window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_PlayerDataRecieved", data);
		}).catch((reason) => {

		});
	}
});