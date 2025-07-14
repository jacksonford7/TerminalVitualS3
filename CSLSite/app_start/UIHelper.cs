using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace CSLSite.app_start
{
    public class UIHelper
    {
        public static string returnOverLay(string innerht, int numero)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<div   id='ocultar{0}'  class='overlay' ><div>", numero);
            sb.Append(innerht);
            sb.AppendFormat(" [<a href='#' onclick='overlay(ocultar{0})'>Cerrar</a>] </div></div>", numero);
            return sb.ToString();

        }

        public  static string remove_invalid_path_char(string orig)
        {
            orig = orig.Trim();
            Regex replace_vars = new Regex(@"[&|*|?|""""|'|#|%|<|>|¡|{|}|~|!|¿|@|-|_]", RegexOptions.Compiled);
            orig = replace_vars.Replace(orig, string.Empty);
            replace_vars = new Regex( "\\s+");
            orig = replace_vars.Replace(orig, string.Empty);
            return orig;
        }

    }
}