using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Data;
using System.Net;
using System.Net.NetworkInformation;

namespace CSLSite
{
    public static class utilform
    {
       public static void MessageBox(String msg, Page page)
        {
            StringBuilder s = new StringBuilder();
            msg=msg.Replace("'"," ");
            msg=msg.Replace(System.Environment.NewLine," ");
            msg=msg.Replace("/","");
            s.Append(" alert('").Append(msg).Append("');");
            System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), s.ToString(), true);
            //    'ScriptManager.RegisterStartupScript(page, GetType(String), "msg", s.ToString(), False)
        }

       public static String ObteneripRequest()
       {

           string strHostName = HttpContext.Current.Request.UserHostAddress;
           IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
           IPAddress ipAddress = ipHostInfo.AddressList[0];
           return ipAddress.ToString();
       }

       public static Boolean ConsXml(String nodo, String paramSting, String wxml, out String paramValor)
       {
           XmlDocument docXML = new XmlDocument();
           String transaccion;
           paramValor = null;
           docXML.LoadXml(wxml);
           XmlElement elemTrans;
           elemTrans = docXML.DocumentElement;

           foreach (XmlNode ItemNode in elemTrans.ChildNodes)
           {
               if (ItemNode.GetType().ToString().Equals("System.Xml.XmlElement"))
               {
                   XmlElement elemItem = (XmlElement)ItemNode;
                   if (elemItem.Name.Equals(nodo))
                   {
                       transaccion = elemItem.GetAttribute(paramSting);
                       if (transaccion != "")
                       {
                           paramValor = transaccion;
                           return true;
                       };
                   };

               };
           }

           return false;
       }

       public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
       {
           DataTable dtReturn = new DataTable();

           // column names 
           PropertyInfo[] oProps = null;

           if (varlist == null) return dtReturn;

           foreach (T rec in varlist)
           {
               if (oProps == null)
               {
                   oProps = ((Type)rec.GetType()).GetProperties();
                   foreach (PropertyInfo pi in oProps)
                   {
                       Type colType = pi.PropertyType;

                       if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                       {
                           colType = colType.GetGenericArguments()[0];
                       }

                       dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                   }
               }

               DataRow dr = dtReturn.NewRow();

               foreach (PropertyInfo pi in oProps)
               {
                   dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                   (rec, null);
               }

               dtReturn.Rows.Add(dr);
           }
           return dtReturn;
       }
        
        public static String Obtenerip(){
                IPHostEntry host;
                String localIP = "";
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                    {
                        if (ip.AddressFamily.ToString() == "InterNetwork")
                        {
                            localIP = ip.ToString();
                         }
                    }
                return localIP;
        }
        
    }
}