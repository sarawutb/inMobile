using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public partial class ResponseApi<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("statusCode")]
        public long StatusCode { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("messenger")]
        public string Messenger { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}