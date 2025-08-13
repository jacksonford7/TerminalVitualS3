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

            int ppm = Math.Max(4, size / 64);

            using (var gen = new QRCodeGenerator())
            using (var dataQr = gen.CreateQrCode(data, level))
            using (var qr = new QRCode(dataQr))
            using (var rawBmp = qr.GetGraphic(ppm, Color.Black, Color.White, true))
            {
                int newW = rawBmp.Width + (margin * 2);
                int newH = rawBmp.Height + (margin * 2);

                using (var finalBmp = new Bitmap(newW, newH))
                using (var g = Graphics.FromImage(finalBmp))
                {
                    g.Clear(Color.White);
                    g.DrawImage(rawBmp, margin, margin);

                    ctx.Response.ContentType = "image/png";
                    finalBmp.Save(ctx.Response.OutputStream, ImageFormat.Png);
                }
            }
        }

        public bool IsReusable => true;
    }
}
