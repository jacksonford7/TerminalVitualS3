using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;


namespace ClsOrdenesP2D
{
    public class delete_order : ToStringBase
    {
      
        [JsonIgnoreAttribute]
        public string token { get; set; }

        [JsonIgnoreAttribute]
        public string UrlOp { get; set; }

        [JsonIgnoreAttribute]
        public string Id_Order { get; set; }

        public delete_order(string _token, string _uop, string _Id_Order)
        {
            this.UrlOp = _uop;
            this.token = _token;
            this.Id_Order = _Id_Order;
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

            if (string.IsNullOrEmpty(this.Id_Order))
            {
                return new messages_order() { code = "ERROR", message = "Debe especificar un id de la orden a eliminar" };
            }

            return runWriteOperation();
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

                    HttpResponseMessage result = client.DeleteAsync(string.Format("{0}/{1}",this.UrlOp, this.Id_Order)).Result;

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        string returnValue = result.Content.ReadAsStringAsync().Result;

                        order_result rs = new order_result();
                        rs = JsonConvert.DeserializeObject<order_result>(returnValue);

                        return new messages_order()
                        {
                            code = "OK",
                            message = returnValue,
                            id = this.Id_Order,
                            order_number = string.Empty,
                            tracking_number = string.Empty
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
