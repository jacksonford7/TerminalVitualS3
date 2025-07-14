using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Xml;
namespace ClSPOLTOIS
{
  public   class ClSUTIL
    {
      public Boolean argo_response( String xlmparameter)
      {
          System.Xml.Linq.XDocument xdoc;

          Boolean wresult = false;

          try
          {


              xdoc = System.Xml.Linq.XDocument.Parse(xlmparameter);

                 var Status = (from row in xdoc.Descendants("argo-response")
                            select new
                            {
                                Status = (String)row.Attribute("status-id").Value,
                            }).FirstOrDefault();

                 if (Status.Status.Equals("OK") == true || Status.Status.Equals("WARNING") == true)
              {
                  wresult = true;
              }
          }
          catch (Exception exc)
          {
              wresult = false;
          }
          return wresult;

      }
      public  DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
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

      public   Boolean  ConsXml(String nodo, String paramSting , String wxml, out String paramValor ) {
        XmlDocument docXML = new XmlDocument();
        String transaccion;
        paramValor=null;
        docXML.LoadXml(wxml);
        XmlElement elemTrans; 
        elemTrans = docXML.DocumentElement;
        
        foreach (XmlNode ItemNode in elemTrans.ChildNodes)
	{
		 if(ItemNode.GetType().ToString().Equals("System.Xml.XmlElement"))
         {
             XmlElement elemItem=(XmlElement)ItemNode;
             if (elemItem.Name.Equals(nodo)){
                 transaccion=elemItem.GetAttribute(paramSting);
                 if (transaccion!=""){
                     paramValor=transaccion;
                     return true;
                 };
             };

         };
	}

        return false;
      }
    }
}
