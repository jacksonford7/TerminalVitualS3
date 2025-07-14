using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClsOrdenesP2D
{
    [Serializable]
    public class order_result
    {
        public result_create_order order { get; set; }
    }

    public class result_create_order
    {
        public string id { get; set; }

        public string order_number { get; set; }

        public string tracking_number { get; set; }

        public string valid { get; set; }
    }
}
