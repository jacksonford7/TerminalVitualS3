using System;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;

namespace Barcode.handler
{
    public class qr : IHttpHandler
    {
        public void ProcessRequest(HttpContext ctx)
        {
            var data = ctx.Request["data"];
            if (string.IsNullOrWhiteSpace(data))
            {
                ctx.Response.StatusCode = 400;
                ctx.Response.Write("Missing data");
                return;
            }

            int size = int.TryParse(ctx.Request["size"], out var s) ? s : 256;
            int margin = int.TryParse(ctx.Request["margin"], out var m) ? m : 2;
            var ecc = (ctx.Request["ecc"] ?? "M").ToUpperInvariant();

            var level = QRCodeGenerator.ECCLevel.M;
            if (ecc == "L") level = QRCodeGenerator.ECCLevel.L;
            else if (ecc == "Q") level = QRCodeGenerator.ECCLevel.Q;
            else if (ecc == "H") level = QRCodeGenerator.ECCLevel.H;

            using (var generator = new QRCodeGenerator())
            using (var dataQr = generator.CreateQrCode(data, level))
            using (var qr = new QRCode(dataQr))
            using (var bmp = qr.GetGraphic(Math.Max(4, size / 64), Color.Black, Color.White, true, margin))
            {
                ctx.Response.ContentType = "image/png";
                bmp.Save(ctx.Response.OutputStream, ImageFormat.Png);
            }
        }

        public bool IsReusable => true;
    }
}
