using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace CSLSite.app_start
{
    public class UIHelper
    {
        public static string returnOverLay(string innerht,int numero)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<div   id='ocultar{0}'  class='overlay' ><div>", numero);
            sb.Append(innerht);
            sb.AppendFormat(" [<a href='#' onclick='overlay(ocultar{0})'>Cerrar</a>] </div></div>",numero);
            return sb.ToString();

        }

    }
}