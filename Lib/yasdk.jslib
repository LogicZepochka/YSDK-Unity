mergeInto(LibraryManager.library, {

	InitFinish: function (gameName,version,ysdkVersion) {
		console.group("YSDK-UNITY START")
		console.log(`Game: ${gameName}`)
		console.log(`Version: ${version}`)
		console.log(`YSDK-Version: ${ysdkVersion}`)
		window.ysdk.features.LoadingAPI?.ready();
	},

	InitStart: function () {
		YaGames
		  .init(params)
		  .then(_sdk => {
			window.ysdk = _sdk;
			ysdk.getPlayer().then(_player => {
				window.player = _player;
				var playerObject = {
					name: "",
					uid: "",
					sphoto: "",
					mphoto: "",
					lphoto: "",
					auth: false
				};
				if(_player.getMode() === 'lite') {
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
				let jsonPlayer = json.stringify(playerObject);
				window.unityInstance.SendMessage("YandexSDKBridge","YSDK_InitPlayer",jsonPlayer);
			});
		  })
		  .catch(console.error);
	}
});