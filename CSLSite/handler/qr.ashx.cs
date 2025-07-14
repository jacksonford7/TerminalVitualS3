using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Text;
using Gma.QrCodeNet;
using System.IO;
using System.Web.UI;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing.Imaging;

namespace Barcode
{
    /// <summary>
    /// Descripción breve de barcode
    /// </summary>
    public class qr : IHttpHandler
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
                    //using (bitmap)
                    {
                        //var grafic = Graphics.FromImage(bitmap);
                        var route = string.Empty;
                        route = System.Configuration.ConfigurationManager.AppSettings["PATH_FONTS"] == null ? "../handler/Fuentes" : System.Configuration.ConfigurationManager.AppSettings["PATH_FONTS"];
                        //var point = new Point();
                        var brush = new SolidBrush(Color.Black);
                        try
                        {
                            context.Response.ContentType = "image/jpeg";

                            var qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                            var qrCode = qrEncoder.Encode(cd);
                            var renderer = new GraphicsRenderer(new FixedModuleSize(size, QuietZoneModules.Two), Brushes.Black, Brushes.White);

                            renderer.WriteToStream(qrCode.Matrix, ImageFormat.Jpeg, context.Response.OutputStream);
                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterStartupScript(new Page(), GetType(), "Show Modal Popup", ex.Message, true);
                            context.Response.Write(string.Empty);
                            throw;
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
    }
}