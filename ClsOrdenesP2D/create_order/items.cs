using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace ClsOrdenesP2D
{
    public class items : ToStringBase
    {
        [JsonProperty] public string description { get; set; }
        [JsonProperty] public string sku { get; set; }
        [JsonProperty] public string barcode { get; set; }
        [JsonProperty] public int quantity { get; set; }
        [JsonProperty] public decimal unit_weight { get; set; }
        [JsonProperty] public decimal unit_volume { get; set; }
        [JsonProperty] public decimal unit_price { get; set; }
        [JsonProperty] public decimal total_price { get; set; }
    }
}
