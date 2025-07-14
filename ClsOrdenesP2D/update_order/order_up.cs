using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace ClsOrdenesP2D
{
    public class order_up : ToStringBase
    {

        [JsonProperty]
        public int hub_id { get; set; }

        [JsonProperty]
        public string order_id { get; set; }

        [JsonProperty]
        public DateTime initial_delivery_time { get; set; }

        [JsonProperty]
        public DateTime finish_delivery_time { get; set; }

        [JsonProperty]
        public string start_time { get; set; }

        [JsonProperty]
        public string end_time { get; set; }

        [JsonProperty]
        public string latitude { get; set; }

        [JsonProperty]
        public string longitude { get; set; }

        [JsonProperty]
        public string neighborhood { get; set; }

        [JsonProperty]
        public string city { get; set; }

        [JsonProperty]
        public string state { get; set; }

        [JsonProperty]
        public string order_number { get; set; }

        [JsonProperty]
        public string zone { get; set; }

        [JsonProperty]
        public string observations { get; set; }

        [JsonProperty]
        public customer customer { get; set; }

        [JsonProperty]
        public List<items> items { get; set; }

        [JsonProperty]
        public meta meta { get; set; }

        public order_up()
        {

            this.items = new List<items>();

        }

    }
}
