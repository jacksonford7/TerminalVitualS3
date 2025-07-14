using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSLSite.Cls_pase_puerta
{
    public class HttpTool
    {
        public static Stream BarcodeStream(Int64 secuencia, string destino_url, string barra_url)
        {
            try
            {
                //https://www.cgsa.com.ec/carbono-neutro/

                string url_destino;
                if (string.IsNullOrEmpty(destino_url))
                {
                    url_destino = string.Format("{1}", destino_url, secuencia);
                }
                else
                {
                    url_destino = string.Format("{0}?{1}", destino_url, secuencia);
                }


                string server_url = string.Format("http://{0}/barcode/handler/qr.ashx?code={1}&format=E9", barra_url, url_destino);
                Stream stream = null;
                string url = server_url;
                System.Net.HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
                if (fileReq.ContentLength > 0)
                { fileResp.ContentLength = fileReq.ContentLength; }
                stream = fileResp.GetResponseStream();
                return stream;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Stream BarStream(Int64 secuencia, string barra_url)
        {
            try
            {
                string server_url = string.Format("http://{0}/barcode/handler/barcode.ashx?code={1}&format=E9&width=280&height=70", barra_url, secuencia);
                Stream stream = null;
                string url = server_url;
                System.Net.HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
                if (fileReq.ContentLength > 0)
                { fileResp.ContentLength = fileReq.ContentLength; }
                stream = fileResp.GetResponseStream();
                return stream;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static Stream BarStream(string secuencia, string barra_url)
        {
            try
            {
                string server_url = string.Format("http://{0}/barcode/handler/barcode.ashx?code={1}&format=E9&width=280&height=70", barra_url, secuencia);
                Stream stream = null;
                string url = server_url;
                System.Net.HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
                if (fileReq.ContentLength > 0)
                { fileResp.ContentLength = fileReq.ContentLength; }
                stream = fileResp.GetResponseStream();
                return stream;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static string normalizeWS(string str)
        {
            return Regex.Replace(str, @"\r\n?|\n", " ");
        }

        /*
        public static Stream BarcodeStream(Int64 secuencia, string destino_url, string barra_url)
        {
            try
            {
                //https://www.cgsa.com.ec/carbono-neutro/
                string url_destino = string.Format("{0}?{1}", destino_url, secuencia);
                string server_url = string.Format("http://{0}/barcode/handler/qr.ashx?code={1}&format=E9", barra_url, url_destino);
                Stream stream = null;
                string url = server_url;
                System.Net.HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
                if (fileReq.ContentLength > 0)
                { fileResp.ContentLength = fileReq.ContentLength; }
                stream = fileResp.GetResponseStream();
                return stream;
            }
            catch
            {
                return null;
            }
        }
        */


    }
}