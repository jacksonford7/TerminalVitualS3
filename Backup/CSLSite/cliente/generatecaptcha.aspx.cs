using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
 

namespace CSLSite.cliente
{
    public partial class generatecaptcha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Create a CAPTCHA image using the text stored in the Session object.
                RandomImage ci = new RandomImage(this.Session
                    ["CaptchaImageText"].ToString(), 325, 73);
                // Change the response headers to output a JPEG image.
                this.Response.Clear();
                this.Response.ContentType = "image/jpeg";
                // Write the image to the response stream in JPEG format.
                ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);
                // Dispose of the CAPTCHA image object.
                ci.Dispose();
            }
            catch (Exception)
            {
                Response.Write("<script language='JavaScript'>var r=alert('Su sesión esta caducada, intente nuevamente por favor.');if(r==true){window.location='http://www.cgsa.com.ec/inicio.aspx';}else{window.location='http://www.cgsa.com.ec/inicio.aspx';}</script>");
            }            
        }
    }
}