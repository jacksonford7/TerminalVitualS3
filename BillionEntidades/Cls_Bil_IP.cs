using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace BillionEntidades
{
   public class Cls_Bil_IP
    {

        public static string GetLocalIPAddress()
        {
            string IP = string.Empty;

            try
            {

              
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    IP = string.Empty;
                    return IP;
                }

                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        IP = ip.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                IP = ex.Message;
            }
            return IP;
        }


    }
}
