using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ClsOrdenesP2D
{
    public class customer : ToStringBase
    {
        [JsonProperty] public string contact { get; set; }
        [JsonProperty] public string name { get; set; }
        [JsonProperty] public string address { get; set; }
        [JsonProperty] public string phone { get; set; }
        [JsonProperty] public string identification_number { get; set; }
        [JsonProperty] public string email { get; set; }
        [JsonProperty] public string extra_address { get; set; }
        [JsonProperty] public string internal_id { get; set; }
    }
}
