
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;


using System.Net.Http;
using System.Net.Http.Headers;

namespace ClsOrdenesP2D
{
    public class update_order : ToStringBase
    {
        [JsonProperty]
        public order_up order { get; set; }

        [JsonIgnoreAttribute]
        public string token { get; set; }

        [JsonIgnoreAttribute]
        public string UrlOp { get; set; }

        public update_order(string _token, string _uop)
        {
            this.UrlOp = _uop;
            this.token = _token;
        }

        public messages_order WriteApiOrder()
        {

            if (string.IsNullOrEmpty(this.token))
            {
                return new messages_order() { code = "ERROR", message = "No existe token" };

            }

            if (string.IsNullOrEmpty(this.UrlOp))
            {
                return new messages_order() { code = "ERROR", message = "URL es elemeneto nulo" };
            }

            return runWriteOperation();
        }

        public string json()
        {
            try
            {
                string data = string.Format("{0}", this.ToString());
                return data;
            }
            catch (Exception Ex)
            {

                return Ex.Message;

            }
        }

        private messages_order runWriteOperation()
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpClientHandler handler = new HttpClientHandler();

                using (var client = new HttpClient(handler, false))
                {
                    client.CancelPendingRequests();

                    client.DefaultRequestHeaders.Clear();
                    client.BaseAddress = new Uri(this.UrlOp);
                    client.Timeout = TimeSpan.FromSeconds(120);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", this.token);
                    client.DefaultRequestHeaders.ConnectionClose = true;


                    string data = string.Format("{0}", this.ToString());
                    var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage result = client.PutAsync(this.UrlOp, content).Result;

                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {

                        string returnValue = result.Content.ReadAsStringAsync().Result;

                        order_result rs = new order_result();
                        rs = JsonConvert.DeserializeObject<order_result>(returnValue);

                        return new messages_order()
                        {
                            code = "OK",
                            message = returnValue,
                            id = rs.order.id,
                            order_number = rs.order.order_number,
                            tracking_number = rs.order.tracking_number
                        };

                    }
                    else
                    {
                        string returnValue = result.Content.ReadAsStringAsync().Result;

                        return new messages_order() { code = "ERROR", message = returnValue };

                    }


                }
            }
            catch (WebException Ex)
            {

                return new messages_order() { code = "ERROR", message = Ex.Message };
            }

        }

    }
}
