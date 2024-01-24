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
			window.ysdk.getLeaderboards().then(_lb => window.ysdkLB = _lb);
			window.ysdk.getPlayer().then(_player => {
				window.player = _player;
				let playerObject = {
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

	GetEnviroment: function () {
		let id = window.ysdk.environment.app.id;
		let lang = window.ysdk.environment.i18n.lang;
		let payload = window.ysdk.environment.i18n.payload;
		let device = 10;
		switch (window.ysdk.deviceInfo.type) {
			case "desktop": { device = 0; break }
			case "mobile": { device = 1; break }
			case "tablet": { device = 2; break }
			case "tv": { device = 3; break }
		}
		if (payload != null) {
			window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_InitEnviroment", id, lang, device, payload);
		}
		else {
			window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_InitEnviroment", id, lang, device, "");
		}
	},

	// YaLeaderboards
	RequestLeaderboardDescription: function (lbName) {
		window.ysdkLB.getLeaderboardDescription(${UTF8ToString(lbName)})
			.then(res => {
				let lbData = {
					Default: false,
					InvertSortOrder: false,
					Type: "",
					Name: "",
					LocNameMap: []
				};

				lbData.Default = res.default;
				lbData.InvertSortOrder = res.description.inver_sort_order;
				lbData.Type = res.description.type;
				lbData.Name = res.name;

				let locKeys = Object.keys(res.title);
				locKeys.forEach(key => {
					lbData.lbDataMap.push(key + ":" + res.title[key]);
				});
				window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_LeaderboardDescriptionRecived", JSON.stringify(lbData));
			});
	},

	SetLeaderboardScore: function (lbName,value) {
		window.ysdkLB.setLeaderboardScore(${UTF8ToString(lbName)}, value);
	},

	

	GetLeaderboardPlayerRating: function (lbName) {
		let lbRatingData = {
			Score: 0,
			Rank: 0,
			Error: false,
			ErrorMessage: ""
		};
		window.ysdkLB.getLeaderboardPlayerEntry(${UTF8ToString(lbName)})
			.then(res => {
				lbRatingData.Score = res.score;
				lbRatingData.Rank = res.rank;
				window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_LeaderboardPlayerRatingRecived", JSON.stringify(lbRatingData));
			})
			.catch(err => {
				lbRatingData.Error = true;
				lbRatingData.ErrorMessage = err.code;
			});
	},

	GetLeaderboardRating: function (lbName, aroundPlayer, top, includePlayer) {

		let proceedData = function (res) {
			let lbData = {
				IncludePlayer: false,
				UserRank: 0,
				Entries: [
					{ rank: 0, playerName: "", avatarSrcSm: "", avatarSrcMd: "", avatarSrcLg: "", score: 0 }
				]
			};
			lbData.IncludePlayer = includePlayer;
			lbData.UserRank = res.userRank;

			res.entries.forEach(entry => {
				lbData.Entries.push({
					rank: entry.rank,
					score: entry.score,
					playerName: entry.player.publicName,
					avatarSrcSm: entry.player.getAvatarSrc("small"),
					avatarSrcMd: entry.player.getAvatarSrc("medium "),
					avatarSrcLg: entry.player.getAvatarSrc("large")
				});
			});

			window.unityInstance.SendMessage("YandexSDKBridge", "YSDK_LeaderboardRatingRecived", JSON.stringify(lbData));
		};

		if (includePlayer) {
			window.ysdkLB.getLeaderboardEntries(${ UTF8ToString(lbName) }, { quantityTop: top, quantityAround: aroundPlayer, includeUser: true })
				.then(res => {
					proceedData(res);
				});
		}
		else {
			window.ysdkLB.getLeaderboardEntries(${ UTF8ToString(lbName) }, { quantityTop: top })
				.then(res => {
					proceedData(res);
				});
		}
	}
});