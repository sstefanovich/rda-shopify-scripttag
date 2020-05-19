(function () {
	var rdaShopifyScript = {
		Settings: {},
		Start: function () {
			var shop = Shopify.shop;
			rdaShopifyScript.LoadSettings(shop, function (settings) {
				//Save app settings
				rdaShopifyScript.Settings = settings;
				alert(settings.WelcomeMessage);
			});
		},
		ExecuteJSONP: function (url, parameters, callback) {
			var callbackName = "MyAppJSONPCallback" + new Date().getMilliseconds();
			window[callbackName] = callback;

			var kvps = ["callback=" + callbackName];
			var keys = Object.getOwnPropertyNames(parameters);

			for (var i = 0; i < keys.length; i++) {
				var key = keys[i];
				kvps.push(key + "=" + parameters[key]);
			}

			kvps.push("uid=" + new Date().getMilliseconds());

			var qs = "?" + kvps.join("&");

			var script = document.createElement("script");
			script.src = url + qs;
			script.async = true;
			script.type = "text/javascript";

			document.head.appendChild(script);
		},
		LoadSettings: function (shop, callback) {
			var settingsLoaded = function (settings) {
				callback(settings);
			};

			rdaShopifyScript.ExecuteJSONP("https://3ddc7a39.ngrok.io/api/settings", { shop: shop }, settingsLoaded);
		},
		SubmitHandler: function (firstName, emailAddress) {
			//This function handles the event when a visitor submits the form.
		}
	};

	rdaShopifyScript.Start();

	window["RdaShopifyScript"] = rdaShopifyScript;
}());