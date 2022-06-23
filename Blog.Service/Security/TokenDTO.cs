using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Security
{
    public class TokenDTO
    {

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

    }
}
