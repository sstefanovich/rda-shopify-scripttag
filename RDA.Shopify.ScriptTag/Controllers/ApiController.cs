using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShopifySharp;
using ShopifySharp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDA.Shopify.ScriptTag.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;
        private readonly IConfiguration _config;

        public ApiController(ILogger<ApiController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet]
        [Route("shopify")]
        public ActionResult Install([FromQuery] string shop)
        {
            //Here we need to build the authorization Url
            var callbackUrl = _config["CallbackUrl"];
            var apiKey = _config["Shopify_API_Key"];
            var redirectUri = $"{callbackUrl}/shopify/callback";

            var scopes = new List<AuthorizationScope>()
            {
                AuthorizationScope.ReadCustomers,
                AuthorizationScope.WriteCustomers,
                AuthorizationScope.WriteScriptTags,
                AuthorizationScope.ReadScriptTags
            };

            var authUrl = AuthorizationService.BuildAuthorizationUrl(scopes, shop, apiKey, redirectUri);

            return Redirect(authUrl.ToString());
        }

        [HttpGet]
        [Route("shopify/callback")]
        public async Task<ActionResult> Callback([FromQuery] string shop, [FromQuery] string hmac, [FromQuery] string code, [FromQuery] string state)
        {
            //Now retrieve the access token
            var apiKey = _config["Shopify_API_Key"];
            var apiSecret = _config["Shopify_Secret_Key"];
            var callbackUrl = _config["CallbackUrl"];

            string accessToken = await AuthorizationService.Authorize(code, shop, apiKey, apiSecret);

            //Add a script tag
            ScriptTagService svc = new ScriptTagService(shop, accessToken);
            var tag = new ShopifySharp.ScriptTag();
            tag.DisplayScope = "online_store";
            tag.Event = "onload";
            tag.Src = $"{callbackUrl}/js/Rda.Shopify.js";

            var createdTag = await svc.CreateAsync(tag);

            return Ok();
        }
    }
}
