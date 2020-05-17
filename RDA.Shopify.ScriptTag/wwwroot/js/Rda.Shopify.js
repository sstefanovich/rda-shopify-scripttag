(function () {
	var rdaShopifyScript = {
		Settings: {},
		Start: function () {
			var shop = Shopify.shop;
			alert("Hello World! from " + shop);
		},
		ExecuteJSONP: function (url, parameters, callback) {
			
		},
		LoadSettings: function (shop, callback) {
			
		},
		SubmitHandler: function (firstName, emailAddress) {
			//This function handles the event when a visitor submits the form.
		}
	};

	rdaShopifyScript.Start();

	window["RdaShopifyScript"] = rdaShopifyScript;
}());