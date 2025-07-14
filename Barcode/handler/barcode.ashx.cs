using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Text;

namespace Barcode
{
    /// <summary>
    /// Descripción breve de barcode
    /// </summary>
    public class barcode : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var cd = context.Request.QueryString.Get("code");
            var fm = context.Request.QueryString.Get("format");
            var width = (!string.IsNullOrEmpty(context.Request.QueryString.Get("width"))) ? int.Parse(context.Request.QueryString.Get("width")) : 200;
            var height = (!string.IsNullOrEmpty(context.Request.QueryString.Get("height"))) ? int.Parse(context.Request.QueryString.Get("height")) : 60;
            var size = (!string.IsNullOrEmpty(context.Request.QueryString.Get("size"))) ? int.Parse(context.Request.QueryString.Get("size")) : 60;
            if (!string.IsNullOrEmpty(cd))
            {
                using (new System.IO.MemoryStream())
                {
                    var bitmap = new Bitmap(width, height);
                    using (bitmap)
                    {
                        var grafic = Graphics.FromImage(bitmap);
                        var route = string.Empty;
                        route = System.Configuration.ConfigurationManager.AppSettings["PATH_FONTS"] == null ? "../handler/Fuentes" : System.Configuration.ConfigurationManager.AppSettings["PATH_FONTS"];
                        var point = new Point();
                        var brush = new SolidBrush(Color.Black);
                        try
                        {
                            var fuente = CargarFuente(fm, size, context.Server.MapPath(route));
                            grafic.FillRectangle(new SolidBrush(Color.White), 0, 0, width, (float)height);
                            grafic.DrawString(FormatBarCode(cd), fuente, brush, point);
                            context.Response.ContentType = "image/jpeg";
                            bitmap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);                          
                        }
                        catch
                        {
                            context.Response.Write(string.Empty);
                        }
                    }
                }
            }
            else
            {
                context.Response.Write(string.Empty);
            }
                
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string FormatBarCode(string code)
        {
            string barcode = string.Empty;
            barcode = string.Format("*{0}*", code);
            return barcode;
        }
        private Font CargarFuente(string fuente, int size, string route)
        {
            string f = "BARCOD39.TTF";
            switch (fuente)
            {
                case "E39":
                    f = "BARCOD39.TTF";
                    break;
                case "E13":
                    f = "EAN-13.TTF";
                    break;
                case "E9":
                    f = "FRE3OF9X.TTF";
                    break;
            }
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(route + @"\" + f);
            FontFamily fontFamily = pfc.Families[0];
            Font _Font = new Font(fontFamily, (float)size);
            return _Font;

        }

    }
}