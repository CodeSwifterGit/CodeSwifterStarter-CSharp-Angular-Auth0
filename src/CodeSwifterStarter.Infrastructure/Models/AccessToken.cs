using System;
using Newtonsoft.Json;
using RTools_NTS.Util;

namespace CodeSwifterStarter.Infrastructure.Models
{
    public class AccessToken
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        
        [JsonIgnore]
        public DateTime Expires => Created.AddSeconds(ExpiresIn);
        
        [JsonIgnore]
        public DateTime Created { get; }

        public AccessToken()
        {
            Created = DateTime.UtcNow;
        }

        internal bool IsExpired()
        {
            return DateTime.UtcNow > Expires;
        }
    }
}
