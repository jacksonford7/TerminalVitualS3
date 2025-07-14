using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Collections;

namespace CSLSite.facturacion
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AutoComplete : System.Web.Services.WebService
    {
        private DataTable p_drEmpresa
        {
            get
            {
                return (DataTable)Session["drEmpresaPPWebAut"];
            }
            set
            {
                Session["drEmpresaPPWebAut"] = value;
            }

        }

        [WebMethod]
        public string[] GetEmpresaList(string prefixText, int count)
        {
            var prefix = prefixText;

            WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

            DataTable DTRESULT = new DataTable();

            var dtRetorno = pasePuerta.GetEmpresainfoFilter(prefix);

            DTRESULT = dtRetorno; //(DataTable)HttpContext.Current.Session["drEmpresaPPWebAut"];
            if (DTRESULT != null)
            {
                var list = (from currentStat in DTRESULT.Select().AsEnumerable()
                            select currentStat.Field<String>("EMPRESA")).ToList().Take(5);
                string[] prefixTextArray = list.ToArray<string>();
                return prefixTextArray;
            }
            else
            {
                ArrayList myAL = new ArrayList();
                // Add stuff to the ArrayList.
                string[] myArr = (String[])myAL.ToArray(typeof(string));
                string[] prefixTextArray2 = myArr.ToArray<string>();
                return prefixTextArray2;
            }
            //Return Selected Products
            //return prefixTextArray;
        }
    }
}
