using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovation.Mobile.App.Models
{
    public class Printer_Profile
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Printer_Name { get; set; }
        [JsonProperty("printerIpAddress")]
        public string Printer_IP_Address { get; set; }
        [JsonProperty("printerPort")]
        public string Printer_Port { get; set; }
        [JsonProperty("siteId")]
        public int Site_id { get; set; }
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
}
