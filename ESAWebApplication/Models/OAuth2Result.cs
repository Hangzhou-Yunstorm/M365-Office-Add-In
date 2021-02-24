using System;

namespace ESAWebApplication.Models
{
    /// <summary>
    /// OAuth2登录结果
    /// </summary>
    public class OAuth2Result
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string IdToken { get; set; }
        public DateTime ExpireIn { get; set; }

    }

    public class PostOAuth2Result
    {
        public string Flag { get; set; }
        public OAuth2Result Result { get; set; }
    }

}