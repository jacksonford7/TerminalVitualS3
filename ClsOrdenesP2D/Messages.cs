using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;


namespace ClsOrdenesP2D
{
 
    public class messages 
    {

        public string code { get; set; }

        public string message { get; set; }

    }

    public class messages_order
    {

        public string code { get; set; }

        public string message { get; set; }

        public string id { get; set; }


        public string order_number { get; set; }


        public string tracking_number { get; set; }


    }
}
