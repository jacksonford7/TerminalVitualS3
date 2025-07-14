using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace CSLSite
{
    public static class Utils
    {
        public static void mostrarMensaje(this System.Web.UI.Page page, string mensaje)
        {
           
            ScriptManager.RegisterStartupScript(page, page.GetType(), "Script", string.Format(@"<script language='javascript'>alert('{0}');</script>", mensaje), false);
        }


        public static void mostrarMensajeRedireccionando(this System.Web.UI.Page page, string mensaje, string pagina)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "Script", string.Format(@"<script language='javascript'>alert('{0}'); window.location='{1}';</script>", mensaje,pagina), false);
        }
    }
}